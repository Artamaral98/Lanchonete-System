namespace Lanchonete.Domain.Entidades;

public sealed class Usuario : EntityBase
{
    public string Login { get; set; } = string.Empty;
    public byte[] SenhaHash { get; set; } = [];
    public byte[] SenhaSalt { get; set; } = [];
}
