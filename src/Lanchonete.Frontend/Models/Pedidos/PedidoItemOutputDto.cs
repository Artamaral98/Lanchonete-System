namespace Lanchonete.Frontend.Models.Pedidos;

public sealed class PedidoItemOutputDto
{
    public Guid CardapioItemId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal SubtotalItem { get; set; }
}
