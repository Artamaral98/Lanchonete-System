namespace Lanchonete.Domain.Entidades;

public sealed class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CriadoEmUtc { get; set; } = DateTime.UtcNow;
    public List<ItemPedido> Itens { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
}
