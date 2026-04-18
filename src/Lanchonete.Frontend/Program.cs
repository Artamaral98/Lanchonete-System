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

// Configuração do HttpClient Base (sem interceptor para Auth)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5035") });

// Registrar o Handler de Autorização
builder.Services.AddScoped<JwtAuthorizationMessageHandler>();

// Registrar HttpClient Autenticado para serviços que precisam de Token
builder.Services.AddHttpClient("LanchoneteAPI", client => client.BaseAddress = new Uri("http://localhost:5035"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// Injetar o HttpClient nomeado como padrão para os serviços
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("LanchoneteAPI"));

// LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Autenticação
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Serviços de Negócio
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CardapioService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();
