using Lanchonete.Application.Interfaces;
using Lanchonete.Infra.Configuracoes;
using Microsoft.Extensions.Options;

namespace Lanchonete.Infra.Servicos;

public sealed class ValidadorCredencialServico(IOptions<UsuarioPadraoConfiguracao> usuarioPadraoConfiguracao)
    : IValidadorCredencialServico
{
    public bool CredencialValida(string usuario, string senha)
    {
        var dados = usuarioPadraoConfiguracao.Value;
        return string.Equals(usuario, dados.Usuario, StringComparison.Ordinal)
               && string.Equals(senha, dados.Senha, StringComparison.Ordinal);
    }
}
