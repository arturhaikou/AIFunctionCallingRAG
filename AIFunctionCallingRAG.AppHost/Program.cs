using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("Password", secret: true);
var aiKey = builder.AddParameter("AI-KEY", secret: true);
var db = builder.AddSqlServer("sqlserver", password)
    .AddDatabase("Eshop");

var cache = builder.AddRedis("redis");

var qdrant = builder.AddQdrant("qdrant");


// You should create an image for SparseVectorGeneratorApi
builder.AddContainer("sparse-api", "sparse-generator")
    .WithHttpEndpoint(port: 5524, targetPort: 80);

var apiService = builder.AddProject<Projects.AIFunctionCallingRAG_ApiService>("apiservice")
    .WithReference(db)
    .WithReference(qdrant)
    .WithReference(cache)
    .WithEnvironment("Qdrant__Host", qdrant.GetEndpoint("http"))
    .WithEnvironment("Qdrant_Size", "1536")
    .WithEnvironment("SparseVectorGenerator__Host", "http://127.0.0.1:5524")
    .WithEnvironment("AI_KEY", aiKey);

builder.AddProject<Projects.AIFunctionCallingRAG_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithEnvironment("API_URL", "http://localhost:5324")
    .WithEnvironment("AI_KEY", aiKey);

var worker = builder.AddProject<Projects.AIFunctionCallingRAG_Workers>("worker")
    .WithReference(db)
    .WithReference(qdrant)
    .WithEnvironment("Qdrant__Host", qdrant.GetEndpoint("http"))
    .WithEnvironment("Qdrant__Size", "1536")
    .WithEnvironment("SparseVectorGenerator__Host", "http://127.0.0.1:5524")
    .WithEnvironment("API_URL", "http://localhost:5324")
    .WithEnvironment("AI_KEY", aiKey);

builder.Build().Run();
