namespace Lanchonete.Application.Dtos.Autenticacao;

public sealed class LoginOutputDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEmUtc { get; set; }
}
