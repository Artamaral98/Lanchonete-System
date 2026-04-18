namespace Lanchonete.Domain.Entidades;

public sealed class ItemPedido : EntityBase
{
    public Guid CardapioItemId { get; set; }
    public int Quantidade { get; set; }
}
