namespace Lanchonete.Frontend.Models;

public sealed class LoginInputDto
{
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public sealed class LoginOutputDto
{
    public string Token { get; set; } = string.Empty;
}

public sealed class CriarUsuarioInputDto
{
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public sealed class UsuarioOutputDto
{
    public string Token { get; set; } = string.Empty;
}
