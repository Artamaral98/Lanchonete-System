namespace Lanchonete.Frontend.Models.Pedidos;

public sealed class AtualizarPedidoInputDto
{
    public List<PedidoItemInputDto> Itens { get; set; } = [];
}
