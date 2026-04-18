using Lanchonete.Application.Interfaces;
using Lanchonete.Application.Servicos;
using Microsoft.Extensions.DependencyInjection;

namespace Lanchonete.Application.InjecaoDependencia;

public static class InjecaoDependenciaApplication
{
    public static IServiceCollection AdicionarApplication(this IServiceCollection services)
    {
        services.AddScoped<IAutenticacaoAppService, AutenticacaoAppService>();
        services.AddScoped<ICardapioAppService, CardapioAppService>();
        services.AddScoped<IPedidoAppService, PedidoAppService>();
        return services;
    }
}
