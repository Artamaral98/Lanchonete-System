using Lanchonete.Application.Constantes;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Dtos.Pedidos;
using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Entidades;
using Lanchonete.Domain.Enums;
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
            ValidarItensEntrada(input.Itens);

            var pedido = new Pedido();
            pedido.Itens = CriarItensPedido(input.Itens);
            
            ProcessarPrecosPedido(pedido);

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
            var pedido = ObterPedido(id);
            resposta.Dados = MapearPedido(pedido);
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }

    public RespostaOutputDto<List<PedidoOutputDto>> ObterTodosPedidos()
    {
        var resposta = new RespostaOutputDto<List<PedidoOutputDto>>();
        var pedidos = pedidoRepositorio.ObterTodos();

        resposta.Dados = pedidos.Select(MapearPedido).ToList();

        return resposta;
    }

    public RespostaOutputDto<PedidoOutputDto> EditarPedido(Guid id, AtualizarPedidoInputDto entrada)
    {
        var resposta = new RespostaOutputDto<PedidoOutputDto>();

        try
        {
            var pedido = ObterPedido(id);

            ValidarItensEntrada(entrada.Itens);

            pedido.Itens = CriarItensPedido(entrada.Itens);
            
            ProcessarPrecosPedido(pedido);

            pedidoRepositorio.Atualizar(pedido);
            resposta.Dados = MapearPedido(pedido);
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }

    private void ProcessarPrecosPedido(Pedido pedido)
    {
        var itensCardapio = pedido.Itens
            .ToDictionary(i => i.CardapioItemId, i => ObterItemCardapio(i.CardapioItemId));

        var subtotal = CalcularSubtotal(pedido.Itens, itensCardapio);
        var descontoPercentual = CalcularDescontoPercentual(itensCardapio.Values);

        pedido.Subtotal = subtotal;
        pedido.Desconto = subtotal * descontoPercentual;
        pedido.Total = subtotal - pedido.Desconto;
    }

    private decimal CalcularDescontoPercentual(IEnumerable<CardapioItem> itensCardapio)
    {
        var categorias = itensCardapio.Select(i => i.Categoria).ToHashSet();

        if (!categorias.Contains(CategoriaItemCardapio.Sanduiche))
        {
            return 0m;
        }

        var temBatata = categorias.Contains(CategoriaItemCardapio.Batata);
        var temBebida = categorias.Contains(CategoriaItemCardapio.Bebida);

        return (temBatata, temBebida) switch
        {
            (true, true)   => 0.20m,
            (false, true)  => 0.15m,
            (true, false)  => 0.10m,
            _              => 0m
        };
    }

    public RespostaOutputDto<bool> RemoverPedido(Guid id)
    {
        var resposta = new RespostaOutputDto<bool>();

        try
        {
            var pedido = ObterPedido(id);
            pedidoRepositorio.Remover(pedido.Id);
            resposta.Dados = true;
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }

    private Pedido ObterPedido(Guid id)
    {
        var pedido = pedidoRepositorio.ObterPorId(id);
        if (pedido == null)
            throw new BusinessException(Messages.PedidoNaoEncontrado);

        return pedido;
    }

    private void ValidarItensEntrada(List<PedidoItemInputDto> itens)
    {
        if (itens.Count == 0)
            throw new BusinessException(Messages.PedidoSemItens);

        var possuiItensDuplicados = itens
            .GroupBy(x => x.CardapioItemId)
            .Any(x => x.Count() > 1);

        if (possuiItensDuplicados)
            throw new BusinessException(Messages.PedidoComItensDuplicados);

        var quantidadeSanduiches = 0;
        foreach (var item in itens)
        {
            var itemCardapio = ObterItemCardapio(item.CardapioItemId);
            if (itemCardapio.Categoria == CategoriaItemCardapio.Sanduiche)
                quantidadeSanduiches += item.Quantidade;
        }

        if (quantidadeSanduiches > 1)
            throw new BusinessException(Messages.PedidoMuitosSanduiches);
    }

    private List<ItemPedido> CriarItensPedido(List<PedidoItemInputDto> itens) =>
        itens.Select(x => new ItemPedido
        {
            CardapioItemId = x.CardapioItemId,
            Quantidade = x.Quantidade
        }).ToList();

    private decimal CalcularSubtotal(
        IEnumerable<ItemPedido> itensPedido,
        Dictionary<Guid, CardapioItem> itensCardapio)
    {
        return itensPedido.Sum(item => itensCardapio[item.CardapioItemId].Preco * item.Quantidade);
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
            Codigo = pedido.Codigo,
            DataCriacao = pedido.DataCriacao,
            Itens = itens,
            Subtotal = pedido.Subtotal,
            Desconto = pedido.Desconto,
            Total = pedido.Total
        };
    }
}
