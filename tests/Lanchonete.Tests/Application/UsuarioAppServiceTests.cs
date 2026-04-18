using Lanchonete.Application.Dtos.Usuarios;
using Lanchonete.Application.Interfaces;
using Lanchonete.Application.Servicos;
using Lanchonete.Domain.Entidades;
using Xunit;

namespace Lanchonete.Tests.Application;

public sealed class UsuarioAppServiceTests
{
    private readonly UsuarioRepositorioFake _usuarioRepositorio = new();
    private readonly CriptografiaServicoFake _criptografiaServico = new();
    private readonly GeradorTokenServicoFake _geradorTokenServico = new();
    private readonly UsuarioAppService _usuarioAppService;

    public UsuarioAppServiceTests()
    {
        _usuarioAppService = new UsuarioAppService(
            _usuarioRepositorio,
            _criptografiaServico,
            _geradorTokenServico);
    }

    [Fact]
    public void CriarUsuario_DeveCriptografarESalvarNoRepositorio()
    {
        // Arrange
        var input = new CriarUsuarioInputDto { Login = "novo_usuario", Senha = "senha123" };

        // Act
        var resposta = _usuarioAppService.CriarUsuario(input);

        // Assert
        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal("token_fake", resposta.Dados.Token);
        Assert.True(_criptografiaServico.CriarHashChamado);
        Assert.True(_usuarioRepositorio.CriarChamado);
        Assert.True(_geradorTokenServico.GerarTokenChamado);
    }

    [Fact]
    public void CriarUsuario_DeveRetornarErro_QuandoLoginJaExiste()
    {
        // Arrange
        var input = new CriarUsuarioInputDto { Login = "usuario_existente", Senha = "123" };
        _usuarioRepositorio.UsuarioExistente = new Usuario { Login = input.Login };

        // Act
        var resposta = _usuarioAppService.CriarUsuario(input);

        // Assert
        Assert.NotEmpty(resposta.Erros);
        Assert.False(_usuarioRepositorio.CriarChamado);
    }

    private sealed class UsuarioRepositorioFake : IUsuarioRepositorio
    {
        public bool CriarChamado { get; private set; }
        public Usuario? UsuarioExistente { get; set; }

        public void Criar(Usuario usuario) => CriarChamado = true;
        public Usuario? ObterPorLogin(string login) => UsuarioExistente;
    }

    private sealed class CriptografiaServicoFake : ICriptografiaServico
    {
        public bool CriarHashChamado { get; private set; }

        public void CriarHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            CriarHashChamado = true;
            senhaHash = [1];
            senhaSalt = [2];
        }

        public bool VerificarHash(string senha, byte[] senhaHash, byte[] senhaSalt) => true;
    }

    private sealed class GeradorTokenServicoFake : IGeradorTokenServico
    {
        public bool GerarTokenChamado { get; private set; }
        public string GerarToken(string usuario)
        {
            GerarTokenChamado = true;
            return "token_fake";
        }
    }
}
