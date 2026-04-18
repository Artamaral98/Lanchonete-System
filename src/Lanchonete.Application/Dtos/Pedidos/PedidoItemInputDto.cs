namespace Lanchonete.Application.Dtos.Pedidos;

public sealed class PedidoItemInputDto
{
    public Guid CardapioItemId { get; set; }
    public int Quantidade { get; set; }
}
