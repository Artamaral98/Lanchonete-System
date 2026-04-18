namespace Lanchonete.Application.Dtos.Pedidos;

public sealed class AtualizarPedidoInputDto
{
    public List<PedidoItemInputDto> Itens { get; set; } = [];
}
