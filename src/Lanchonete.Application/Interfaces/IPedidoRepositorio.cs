using Lanchonete.Domain.Entidades;

namespace Lanchonete.Application.Interfaces;

public interface IPedidoRepositorio
{
    Pedido Criar(Pedido pedido);
    Pedido? ObterPorId(Guid id);
    IEnumerable<Pedido> ObterTodos();
    void Atualizar(Pedido pedido);
    void Remover(Guid id);
}
