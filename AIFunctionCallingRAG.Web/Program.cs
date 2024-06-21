using AIFunctionCallingRAG.Web.ApiClients;
using AIFunctionCallingRAG.Web.Components;
using RestEase.HttpClientFactory;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRestEaseClient<IEshopApi>(builder.Configuration["API_URL"]);
//builder.Services.AddHttpClient<EshopApiClient>(client => client.BaseAddress = new(builder.Configuration["API_URL"]));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

//app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
