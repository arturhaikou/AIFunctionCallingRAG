using AIFunctionCallingRAG.Data.DbContexts;
using AIFunctionCallingRAG.Data.Models;
using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System.Diagnostics;

namespace AIFunctionCallingRAG.Workers;

public class Worker(QdrantClient _client, IConfiguration _configuration, IServiceProvider serviceProvider, ISparseVectorGeneratorApi api) : BackgroundService
{
    private const string ProductsCollectionName = "products";
    private const string ProductsSparseCollectionName = "products-sparse";
    public const string ActivitySourceName = "Worker";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Worker started", ActivityKind.Client);
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EshopDbContext>();
            await MigrateDb(dbContext, stoppingToken);
            await SeedQdrant(dbContext);
            await SeedSparseQdrant(dbContext);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }
    }

    #region DB migration
    private async Task MigrateDb(EshopDbContext dbContext, CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Database migration", ActivityKind.Client);
        await EnsureDatabaseAsync(dbContext, cancellationToken);
        await RunMigrationAsync(dbContext, cancellationToken);

    }

    private static async Task EnsureDatabaseAsync(EshopDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(EshopDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
    #endregion
    private async Task SeedQdrant(EshopDbContext context)
    {
        using var activity = s_activitySource.StartActivity("Seeding Qdrant", ActivityKind.Client);
        if (!await _client.CollectionExistsAsync(ProductsCollectionName))
        {
            var openAIClient = GetOpenAIClient();
            var products = await context.Products.ToListAsync();
            await _client.CreateCollectionAsync(ProductsCollectionName, new VectorParams { Size = _configuration.GetValue<ulong>("Qdrant:Size"), Distance = Distance.Cosine });

            foreach (var product in products)
            {
                var document = GetDocument(product);
                var embeddings = await GetEmbeddingsAsync(openAIClient, document);
                var pointStruct = CreatePointStruct(product, embeddings);
                await _client.UpsertAsync(ProductsCollectionName, [ pointStruct ]);
            }
        }
    }

    private async Task SeedSparseQdrant(EshopDbContext context)
    {
        using var activity = s_activitySource.StartActivity("Seeding Sparse vectors Qdrant", ActivityKind.Client);
        if (!await _client.CollectionExistsAsync(ProductsSparseCollectionName))
        {
            var openAIClient = GetOpenAIClient();
            var products = await context.Products.ToListAsync();
            await _client.CreateCollectionAsync(
                ProductsSparseCollectionName,
                vectorsConfig: new VectorParamsMap { Map = { ["dense"] = new VectorParams { Size = _configuration.GetValue<ulong>("Qdrant:Size"), Distance = Distance.Cosine } } },
                sparseVectorsConfig: new SparseVectorConfig() { Map = { ["sparse"] = new SparseVectorParams { Index = new SparseIndexConfig { OnDisk = false } } } });

            foreach (var product in products)
            {
                var document = GetDocument(product);
                var embeddings = await GetEmbeddingsAsync(openAIClient, document);
                var sparse = await GetSparseValuesAsync(document);
                var pointStruct = CreatePointStruct(product, embeddings, sparse);
                await _client.UpsertAsync(ProductsSparseCollectionName, [ pointStruct ]);
            }
        }
    }

    private string GetDocument(Product product)
    {
        return $"""
                        Id: {product.Id}
                        Name: {product.Name}
                        Description: {product.Description}
                        Color: {product.Color}
                        Size: {product.Size}
                        Price: {product.Price}
                        """;
    }

    private PointStruct CreatePointStruct(Product product, Vectors vectors)
    {
        return new PointStruct
        {
            Id = product.Id,
            Payload =
                    {
                        [nameof(product.Id)] = product.Id.ToString(),
                        [nameof(product.Name)] = product.Name,
                        [nameof(product.Description)] = product.Description,
                        [nameof(product.Color)] = product.Color,
                        [nameof(product.Price)] = product.Price.ToString(),
                        [nameof(product.Size)] = product.Size
                    },
            Vectors = vectors
        };
    }

    private PointStruct CreatePointStruct(Product product, float[] embeddings, (float[], uint[]) sparse)
    {
        return new PointStruct
        {
            Id = product.Id,
            Payload =
                    {
                        [nameof(product.Id)] = product.Id.ToString(),
                        [nameof(product.Name)] = product.Name,
                        [nameof(product.Description)] = product.Description,
                        [nameof(product.Color)] = product.Color,
                        [nameof(product.Price)] = product.Price.ToString(),
                        [nameof(product.Size)] = product.Size
                    },
            Vectors = new Dictionary<string, Vector>
                    {
                        { "sparse", new Vector(sparse) },
                        { "dense", new Vector(embeddings) },
                    },
        };
    }

    private async Task<float[]> GetEmbeddingsAsync(OpenAIClient openAIClient, string input)
    {
        var options = new EmbeddingsOptions
        {
            DeploymentName = "text-embedding-ada-002",
            Input = { input }
        };

        var response = await openAIClient.GetEmbeddingsAsync(options);
        return response.Value.Data.First().Embedding.ToArray();
    }

    private async Task<(float[] Values, uint[] Indices)> GetSparseValuesAsync(string input)
    {
        var response = await api.GetSparseVectorAsync(new SparseVectorRequest { text = input });
        return (response.Values, response.Indices);
    }

    private OpenAIClient GetOpenAIClient()
    {
        var openAIKey = _configuration.GetValue<string>("AI_KEY");
        return new OpenAIClient(openAIKey);
    }
}
