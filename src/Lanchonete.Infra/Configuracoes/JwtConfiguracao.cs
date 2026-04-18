namespace Lanchonete.Infra.Configuracoes;

public sealed class JwtConfiguracao
{
    public const string Secao = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiracaoMinutos { get; set; } = 60;
}
