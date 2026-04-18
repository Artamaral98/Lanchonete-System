using Lanchonete.Application.Constantes;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Dtos.Pedidos;
using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Entidades;
using Lanchonete.Domain.Exceptions;

namespace Lanchonete.Application.Servicos;

public class PedidoAppService(
    IPedidoRepositorio pedidoRepositorio,
    ICardapioRepositorio cardapioRepositorio) : IPedidoAppService
{
    public RespostaOutputDto<PedidoOutputDto> CriarPedido(CriarPedidoInputDto input)
    {
        var resposta = new RespostaOutputDto<PedidoOutputDto>();

        try
        {
            ValidarItensEntrada(input);

            var pedido = new Pedido();
            pedido.Itens = CriarItensPedido(input);
            var subtotal = CalcularSubtotal(pedido.Itens);

            pedido.Subtotal = subtotal;
            pedido.Desconto = 0m;
            pedido.Total = subtotal;

            var pedidoCriado = pedidoRepositorio.Criar(pedido);
            resposta.Dados = MapearPedido(pedidoCriado);
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }

    public RespostaOutputDto<PedidoOutputDto> ObterPedidoPorId(Guid id)
    {
        var resposta = new RespostaOutputDto<PedidoOutputDto>();

        try
        {
            var pedido = pedidoRepositorio.ObterPorId(id);

            if (pedido == null)
                throw new BusinessException(Messages.PedidoNaoEncontrado);

            resposta.Dados = MapearPedido(pedido);
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }

    private static void ValidarItensEntrada(CriarPedidoInputDto input)
    {
        if (input.Itens.Count == 0)
            throw new BusinessException(Messages.PedidoSemItens);

        var possuiItensDuplicados = input.Itens
            .GroupBy(x => x.CardapioItemId)
            .Any(x => x.Count() > 1);

        if (possuiItensDuplicados)
            throw new BusinessException(Messages.PedidoComItensDuplicados);
    }

    private static List<ItemPedido> CriarItensPedido(CriarPedidoInputDto input) =>
        input.Itens.Select(x => new ItemPedido
        {
            CardapioItemId = x.CardapioItemId,
            Quantidade = x.Quantidade
        }).ToList();

    private decimal CalcularSubtotal(IEnumerable<ItemPedido> itensPedido)
    {
        decimal subtotal = 0m;
        foreach (var item in itensPedido)
        {
            var itemCardapio = ObterItemCardapio(item.CardapioItemId);
            subtotal += itemCardapio.Preco * item.Quantidade;
        }

        return subtotal;
    }

    private CardapioItem ObterItemCardapio(Guid cardapioItemId)
    {
        var itemCardapio = cardapioRepositorio.ObterPorId(cardapioItemId);
        if (itemCardapio is null)
            throw new BusinessException(Messages.ItemCardapioNaoEncontrado);

        return itemCardapio;
    }

    private PedidoOutputDto MapearPedido(Pedido pedido)
    {
        var itens = pedido.Itens.Select(item =>
        {
            var itemCardapio = ObterItemCardapio(item.CardapioItemId);
            var preco = itemCardapio.Preco;

            return new PedidoItemOutputDto
            {
                CardapioItemId = item.CardapioItemId,
                NomeItem = itemCardapio.Nome,
                Quantidade = item.Quantidade,
                PrecoUnitario = preco,
                SubtotalItem = preco * item.Quantidade
            };
        }).ToList();

        return new PedidoOutputDto
        {
            Id = pedido.Id,
            DataCriacao = pedido.DataCriacao,
            Itens = itens,
            Subtotal = pedido.Subtotal,
            Desconto = pedido.Desconto,
            Total = pedido.Total
        };
    }
}
