using AIFunctionCallingRAG.ApiService.Apis;
using AIFunctionCallingRAG.ApiService.Services;
using AIFunctionCallingRAG.ApiService.Services.Interfaces;
using AIFunctionCallingRAG.Data.DbContexts;
using Azure.AI.OpenAI;
using Qdrant.Client;
using RestEase.HttpClientFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddSqlServerDbContext<EshopDbContext>("EShop");
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddKeyedTransient<SearchService, DenseSearchService>("dense-search");
builder.Services.AddKeyedTransient<SearchService, HybridSearchService>("hybrid-search");
builder.Services.AddTransient<OpenAIClient>(options => new(builder.Configuration["AI_KEY"]));
builder.AddQdrantClient("qdrant");
builder.Services.AddRestEaseClient<ISparseVectorGeneratorApi>(builder.Configuration["SparseVectorGenerator:Host"]);
builder.AddRedisClient("redis");

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapControllers();
app.Run();