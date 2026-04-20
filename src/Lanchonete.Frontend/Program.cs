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

builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<AuthService>(client => client.BaseAddress = new Uri("http://localhost:5035/"));

builder.Services.AddHttpClient<CardapioService>(client => client.BaseAddress = new Uri("http://localhost:5035/"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddHttpClient<PedidoService>(client => client.BaseAddress = new Uri("http://localhost:5035/"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();