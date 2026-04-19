using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Lanchonete.Frontend;
using Lanchonete.Frontend.Infrastructure;
using Lanchonete.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Handler de autorização (DEVE ser Transient para funcionar com HttpClientFactory)
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Configuração dos Serviços com HttpClients Tipados (já incluem o Handler de Auth)
builder.Services.AddHttpClient<AuthService>(client => client.BaseAddress = new Uri("https://localhost:7247"));

builder.Services.AddHttpClient<CardapioService>(client => client.BaseAddress = new Uri("https://localhost:7247"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<PedidoService>(client => client.BaseAddress = new Uri("https://localhost:7247"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Autenticação
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Serviços
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();