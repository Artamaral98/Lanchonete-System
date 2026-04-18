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
    [Fact]
    public void CriarPedido_DeveCriarComSucesso_QuandoItensValidos()
    {
        var itemCardapio = new CardapioItem
        {
            Id = Guid.NewGuid(),
            Nome = "X Burger",
            Preco = 10m,
            Categoria = CategoriaItemCardapio.Sanduiche
        };

        var appService = new PedidoAppService(
            new PedidoRepositorioFake(),
            new CardapioRepositorioFake([itemCardapio]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto
        {
            Itens = [new PedidoItemInputDto { CardapioItemId = itemCardapio.Id, Quantidade = 2 }]
        });

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal(20m, resposta.Dados.Subtotal);
        Assert.Equal(20m, resposta.Dados.Total);
        Assert.Single(resposta.Dados.Itens);
    }

    [Fact]
    public void CriarPedido_DeveRetornarErro_QuandoSemItens()
    {
        var appService = new PedidoAppService(
            new PedidoRepositorioFake(),
            new CardapioRepositorioFake([]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto { Itens = [] });

        Assert.NotEmpty(resposta.Erros);
        Assert.Contains(Messages.PedidoSemItens, resposta.Erros);
        Assert.Null(resposta.Dados);
    }

    [Fact]
    public void ObterPedidoPorId_DeveRetornarComSucesso_QuandoPedidoExiste()
    {
        var pedido = new Pedido { Id = Guid.NewGuid() };
        var appService = new PedidoAppService(
            new PedidoRepositorioFake(pedido),
            new CardapioRepositorioFake([]));

        var resposta = appService.ObterPedidoPorId(pedido.Id);

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal(pedido.Id, resposta.Dados.Id);
    }

    [Fact]
    public void ObterPedidoPorId_DeveRetornarErro_QuandoPedidoNaoExiste()
    {
        var appService = new PedidoAppService(
            new PedidoRepositorioFake(),
            new CardapioRepositorioFake([]));

        var resposta = appService.ObterPedidoPorId(Guid.NewGuid());

        Assert.NotEmpty(resposta.Erros);
        Assert.Contains(Messages.PedidoNaoEncontrado, resposta.Erros);
        Assert.Null(resposta.Dados);
    }

    private sealed class PedidoRepositorioFake : IPedidoRepositorio
    {
        private readonly List<Pedido> _pedidos = [];

        public PedidoRepositorioFake(params Pedido[] pedidos)
        {
            _pedidos.AddRange(pedidos);
        }

        public Pedido Criar(Pedido pedido)
        {
            if (pedido.Id == Guid.Empty)
                pedido.Id = Guid.NewGuid();
            
            _pedidos.Add(pedido);
            return pedido;
        }

        public Pedido? ObterPorId(Guid id) => _pedidos.FirstOrDefault(p => p.Id == id);
    }

    private sealed class CardapioRepositorioFake(List<CardapioItem> itens) : ICardapioRepositorio
    {
        public List<CardapioItem> ObterTodos() => itens;
        public CardapioItem? ObterPorId(Guid id) => itens.FirstOrDefault(x => x.Id == id);
    }
}
