namespace Lanchonete.Infra.Configuracoes;

public sealed class UsuarioPadraoConfiguracao
{
    public const string Secao = "UsuarioPadrao";

    public string Usuario { get; set; } = "admin";
    public string Senha { get; set; } = "123456";
}
