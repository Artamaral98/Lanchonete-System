using Lanchonete.Application.Interfaces;

namespace Lanchonete.Infra.Servicos;

public sealed class ValidadorCredencialServico(
    IUsuarioRepositorio usuarioRepositorio,
    ICriptografiaServico criptografiaServico)
    : IValidadorCredencialServico
{
    public bool CredencialValida(string usuario, string senha)
    {
        var usuarioDb = usuarioRepositorio.ObterPorLogin(usuario);
        
        if (usuarioDb is null) return false;

        return criptografiaServico.VerificarHash(senha, usuarioDb.SenhaHash, usuarioDb.SenhaSalt);
    }
}
