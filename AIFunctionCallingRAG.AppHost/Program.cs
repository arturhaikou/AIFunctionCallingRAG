using AIFunctionCallingRAG.Hosting;
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Configure parameters
var password = builder.AddParameter("Password", secret: true);
var aiKey = builder.AddParameter("AI-KEY", secret: true);

var db = builder.AddSqlServer("sqlserver", password)
    .WithHealthCheck()
    .AddDatabase("Eshop");

var cache = builder.AddRedis("redis");
var qdrant = builder.AddQdrant("qdrant");

// You should create an image for SparseVectorGeneratorApi
var sparseApi = builder.AddContainer("sparse-api", "sparse-generator")
    .WithHealthCheck(path: "healthcheck")
    .WithHttpEndpoint(port: 5524, targetPort: 80);

var worker = builder.AddProject<Projects.AIFunctionCallingRAG_Workers>("worker")
    .WithReference(db)
    .WithReference(qdrant)
    .WithEnvironment("Qdrant__Size", "1536")
    .WithEnvironment("SparseVectorGenerator__Host", sparseApi.GetEndpoint("http"))
    .WithEnvironment("AI_KEY", aiKey)
    .WaitFor(db)
    .WaitFor(sparseApi);

var apiService = builder.AddProject<Projects.AIFunctionCallingRAG_ApiService>("apiservice")
    .WithReference(db)
    .WithReference(qdrant)
    .WithReference(cache)
    .WithEnvironment("Qdrant__Size", "1536")
    .WithEnvironment("SparseVectorGenerator__Host", sparseApi.GetEndpoint("http"))
    .WithEnvironment("AI_KEY", aiKey);

builder.AddProject<Projects.AIFunctionCallingRAG_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithEnvironment("API_URL", apiService.GetEndpoint("http"))
    .WithEnvironment("AI_KEY", aiKey);

builder.Build().Run();
