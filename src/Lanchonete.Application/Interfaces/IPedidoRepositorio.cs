using Lanchonete.Domain.Entidades;

namespace Lanchonete.Application.Interfaces;

public interface IPedidoRepositorio
{
    Pedido Criar(Pedido pedido);
    Pedido? ObterPorId(Guid id);
}
