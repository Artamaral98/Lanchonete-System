namespace Lanchonete.Application.Dtos.Autenticacao;

public sealed class LoginInputDto
{
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
