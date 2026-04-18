namespace Lanchonete.Application.Dtos.Pedidos;

public sealed class CriarPedidoInputDto
{
    public List<PedidoItemInputDto> Itens { get; set; } = [];
}
