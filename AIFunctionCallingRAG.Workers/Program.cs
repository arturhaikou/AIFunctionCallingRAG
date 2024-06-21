using AIFunctionCallingRAG.Data.DbContexts;
using AIFunctionCallingRAG.Workers;
using Qdrant.Client;
using RestEase.HttpClientFactory;
using System.Net.Http.Headers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<EshopDbContext>("Eshop");
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<QdrantClient>(options => new(builder.Configuration["Qdrant:Host"]));
builder.Services.AddRestEaseClient<ISparseVectorGeneratorApi>(builder.Configuration["SparseVectorGenerator:Host"]);

var host = builder.Build();
host.Run();
