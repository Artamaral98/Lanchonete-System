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
}
