using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Dtos.Pedidos;

namespace Lanchonete.Application.Interfaces;

public interface IPedidoAppService
{
    RespostaOutputDto<PedidoOutputDto> CriarPedido(CriarPedidoInputDto entrada);
    RespostaOutputDto<PedidoOutputDto> ObterPedidoPorId(Guid id);
    RespostaOutputDto<List<PedidoOutputDto>> ObterTodosPedidos();
    RespostaOutputDto<PedidoOutputDto> EditarPedido(Guid id, AtualizarPedidoInputDto entrada);
    RespostaOutputDto<bool> RemoverPedido(Guid id);
}
