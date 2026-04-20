namespace Lanchonete.Frontend.Models.Pedidos;

public sealed class PedidoOutputDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public DateTime DataCriacao { get; set; }
    public List<PedidoItemOutputDto> Itens { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
}
