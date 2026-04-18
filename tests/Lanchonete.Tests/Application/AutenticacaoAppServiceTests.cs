using Lanchonete.Application.Dtos.Autenticacao;
using Lanchonete.Application.Interfaces;
using Lanchonete.Application.Servicos;

namespace Lanchonete.Tests.Application;

public sealed class AutenticacaoAppServiceTests
{
    [Fact]
    public void GerarToken_DeveRetornarToken_QuandoCredencialValida()
    {
        var appService = new AutenticacaoAppService(
            new ValidadorCredencialFake(true),
            new GeradorTokenFake());

        var resposta = appService.GerarToken(new LoginInputDto
        {
            Usuario = "admin",
            Senha = "123456"
        });

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal("token-fake", resposta.Dados!.Token);
    }

    [Fact]
    public void GerarToken_DeveRetornarErro_QuandoCredencialInvalida()
    {
        var appService = new AutenticacaoAppService(
            new ValidadorCredencialFake(false),
            new GeradorTokenFake());

        var resposta = appService.GerarToken(new LoginInputDto
        {
            Usuario = "invalido",
            Senha = "invalido"
        });

        Assert.NotEmpty(resposta.Erros);
        Assert.Null(resposta.Dados);
    }

    private sealed class ValidadorCredencialFake(bool retorno) : IValidadorCredencialServico
    {
        public bool CredencialValida(string usuario, string senha) => retorno;
    }

    private sealed class GeradorTokenFake : IGeradorTokenServico
    {
        public (string Token, DateTime ExpiraEmUtc) GerarToken(string usuario)
            => ("token-fake", DateTime.UtcNow.AddMinutes(30));
    }
}
