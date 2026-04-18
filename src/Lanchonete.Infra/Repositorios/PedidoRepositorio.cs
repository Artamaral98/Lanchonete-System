using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Entidades;

namespace Lanchonete.Infra.Repositorios;

public sealed class PedidoRepositorio : IPedidoRepositorio
{
    private static readonly List<Pedido> Pedidos = [];

    public Pedido Criar(Pedido pedido)
    {
        Pedidos.Add(pedido);
        return pedido;
    }

    public Pedido? ObterPorId(Guid id) => Pedidos.FirstOrDefault(x => x.Id == id);

    public IEnumerable<Pedido> ObterTodos() => Pedidos.AsReadOnly();

    public void Atualizar(Pedido pedido)
    {
        var existente = ObterPorId(pedido.Id);
        if (existente != null)
        {
            existente.Itens = pedido.Itens;
            existente.Subtotal = pedido.Subtotal;
            existente.Desconto = pedido.Desconto;
            existente.Total = pedido.Total;
        }
    }

    public void Remover(Guid id)
    {
        var pedido = ObterPorId(id);
        if (pedido != null)
        {
            Pedidos.Remove(pedido);
        }
    }
}
