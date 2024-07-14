using AIFunctionCallingRAG.Data.DbContexts;
using AIFunctionCallingRAG.Workers;
using RestEase.HttpClientFactory;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<EshopDbContext>("Eshop");
builder.Services.AddHostedService<Worker>();
builder.AddQdrantClient("qdrant");
builder.Services.AddRestEaseClient<ISparseVectorGeneratorApi>(builder.Configuration["SparseVectorGenerator:Host"]);

var host = builder.Build();
host.Run();
