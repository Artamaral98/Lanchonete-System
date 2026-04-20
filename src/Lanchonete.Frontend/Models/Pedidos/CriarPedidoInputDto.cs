namespace Lanchonete.Frontend.Models.Pedidos;

public sealed class CriarPedidoInputDto
{
    public List<PedidoItemInputDto> Itens { get; set; } = [];
}
