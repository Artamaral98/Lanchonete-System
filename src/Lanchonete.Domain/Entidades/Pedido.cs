namespace Lanchonete.Domain.Entidades;

public sealed class Pedido : EntityBase
{
    public int Codigo { get; set; }
    public List<ItemPedido> Itens { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
}
