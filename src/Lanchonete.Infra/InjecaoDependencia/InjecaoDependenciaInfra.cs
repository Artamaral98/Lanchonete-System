using Lanchonete.Application.Interfaces;
using Lanchonete.Infra.Configuracoes;
using Lanchonete.Infra.Repositorios;
using Lanchonete.Infra.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lanchonete.Infra.InjecaoDependencia;

public static class InjecaoDependenciaInfra
{
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtConfiguracao>(configuration.GetSection(JwtConfiguracao.Secao));
        services.Configure<UsuarioPadraoConfiguracao>(configuration.GetSection(UsuarioPadraoConfiguracao.Secao));

        services.AddScoped<IValidadorCredencialServico, ValidadorCredencialServico>();
        services.AddScoped<IGeradorTokenServico, GeradorTokenServico>();
        services.AddSingleton<ICardapioRepositorio, CardapioRepositorio>();
        services.AddSingleton<IPedidoRepositorio, PedidoRepositorio>();

        return services;
    }
}
