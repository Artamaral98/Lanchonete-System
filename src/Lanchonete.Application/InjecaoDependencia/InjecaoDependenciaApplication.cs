using Lanchonete.Application.Interfaces;
using Lanchonete.Application.Servicos;
using Microsoft.Extensions.DependencyInjection;

namespace Lanchonete.Application.InjecaoDependencia;

public static class InjecaoDependenciaApplication
{
    public static IServiceCollection AdicionarApplication(this IServiceCollection services)
    {
        services.AddScoped<IAutenticacaoAppService, AutenticacaoAppService>();
        return services;
    }
}
