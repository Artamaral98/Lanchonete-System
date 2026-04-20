namespace Lanchonete.Frontend.Models.Pedidos;

public sealed class PedidoItemInputDto
{
    public Guid CardapioItemId { get; set; }
    public int Quantidade { get; set; }
}
