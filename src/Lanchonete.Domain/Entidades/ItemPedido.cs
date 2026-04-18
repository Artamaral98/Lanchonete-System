namespace Lanchonete.Domain.Entidades;

public sealed class ItemPedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CardapioItemId { get; set; }
    public int Quantidade { get; set; }
}
