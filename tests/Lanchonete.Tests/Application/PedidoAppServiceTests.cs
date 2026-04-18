using Lanchonete.Application.Constantes;
using Lanchonete.Application.Dtos.Pedidos;
using Lanchonete.Application.Interfaces;
using Lanchonete.Application.Servicos;
using Lanchonete.Domain.Entidades;
using Lanchonete.Domain.Enums;

namespace Lanchonete.Tests.Application;

public sealed class PedidoAppServiceTests
{
    [Fact]
    public void CriarPedido_DeveRetornarErro_QuandoItensDuplicados()
    {
        var itemCardapio = new CardapioItem
        {
            Nome = "X Burger",
            Preco = 5m,
            Categoria = CategoriaItemCardapio.Sanduiche
        };

        var appService = new PedidoAppService(
            new PedidoRepositorioFake(),
            new CardapioRepositorioFake([itemCardapio]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto
        {
            Itens =
            [
                new PedidoItemInputDto { CardapioItemId = itemCardapio.Id, Quantidade = 1 },
                new PedidoItemInputDto { CardapioItemId = itemCardapio.Id, Quantidade = 2 }
            ]
        });

        Assert.NotEmpty(resposta.Erros);
        Assert.Contains(Messages.PedidoComItensDuplicados, resposta.Erros);
        Assert.Null(resposta.Dados);
    }

    private sealed class PedidoRepositorioFake : IPedidoRepositorio
    {
        public Pedido Criar(Pedido pedido) => pedido;
        public Pedido? ObterPorId(Guid id) => null;
    }

    private sealed class CardapioRepositorioFake(List<CardapioItem> itens) : ICardapioRepositorio
    {
        public List<CardapioItem> ObterTodos() => itens;
        public CardapioItem? ObterPorId(Guid id) => itens.FirstOrDefault(x => x.Id == id);
    }
}
