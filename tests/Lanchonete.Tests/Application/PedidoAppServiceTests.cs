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
        var itemSanduiche = new CardapioItem
        {
            Nome = "X Burger",
            Preco = 5m,
            Categoria = CategoriaItemCardapio.Sanduiche
        };

        var appService = new PedidoAppService(
            new PedidoRepositorioFake(),
            new CardapioRepositorioFake([itemSanduiche]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto
        {
            Itens =
            [
                new PedidoItemInputDto { CardapioItemId = itemSanduiche.Id, Quantidade = 1 },
                new PedidoItemInputDto { CardapioItemId = itemSanduiche.Id, Quantidade = 1 }
            ]
        });

        Assert.NotEmpty(resposta.Erros);
        Assert.Contains(Messages.PedidoComItensDuplicados, resposta.Erros);
        Assert.Null(resposta.Dados);
    }
    [Fact]
    public void CriarPedido_DeveCriarComSucesso_QuandoItensValidos()
    {
        var itemSanduiche = new CardapioItem
        {
            Id = Guid.NewGuid(),
            Nome = "X Burger",
            Preco = 10m,
            Categoria = CategoriaItemCardapio.Sanduiche
        };

        var appService = new PedidoAppService(
            new PedidoRepositorioFake(),
            new CardapioRepositorioFake([itemSanduiche]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto
        {
            Itens = [new PedidoItemInputDto { CardapioItemId = itemSanduiche.Id, Quantidade = 1 }]
        });

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal(10m, resposta.Dados.Subtotal);
        Assert.Equal(10m, resposta.Dados.Total);
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

    [Fact]
    public void CriarPedido_DevePermitirPedidoSemSanduiche_SemDesconto()
    {
        var itemBatata = new CardapioItem { Id = Guid.NewGuid(), Nome = "Batata", Preco = 5m, Categoria = CategoriaItemCardapio.Batata };
        var appService = new PedidoAppService(new PedidoRepositorioFake(), new CardapioRepositorioFake([itemBatata]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto
        {
            Itens = [new PedidoItemInputDto { CardapioItemId = itemBatata.Id, Quantidade = 1 }]
        });

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal(5m, resposta.Dados.Total);
        Assert.Equal(0m, resposta.Dados.Desconto);
    }

    [Fact]
    public void CriarPedido_DeveRetornarErro_QuandoMaisDeUmSanduiche()
    {
        var itemSanduiche = new CardapioItem { Id = Guid.NewGuid(), Nome = "X Burger", Categoria = CategoriaItemCardapio.Sanduiche };
        var appService = new PedidoAppService(new PedidoRepositorioFake(), new CardapioRepositorioFake([itemSanduiche]));

        var resposta = appService.CriarPedido(new CriarPedidoInputDto
        {
            Itens = [new PedidoItemInputDto { CardapioItemId = itemSanduiche.Id, Quantidade = 2 }]
        });

        Assert.NotEmpty(resposta.Erros);
        Assert.Contains(Messages.PedidoMuitosSanduiches, resposta.Erros);
    }

    [Theory]
    [InlineData(true, true, 0.20)] // Sanduiche + Batata + Refri = 20%
    [InlineData(false, true, 0.15)] // Sanduiche + Refri = 15%
    [InlineData(true, false, 0.10)] // Sanduiche + Batata = 10%
    [InlineData(false, false, 0.0)] // Só Sanduiche = 0%
    public void CriarPedido_DeveCalcularDescontoCorretamente(bool comBatata, bool comRefri, decimal percentualEsperado)
    {
        var sanduiche = new CardapioItem { Id = Guid.NewGuid(), Nome = "X Burger", Preco = 10m, Categoria = CategoriaItemCardapio.Sanduiche };
        var batata = new CardapioItem { Id = Guid.NewGuid(), Nome = "Batata frita", Preco = 5m, Categoria = CategoriaItemCardapio.Batata };
        var refri = new CardapioItem { Id = Guid.NewGuid(), Nome = "Refrigerante", Preco = 5m, Categoria = CategoriaItemCardapio.Bebida };
        
        var itensCardapio = new List<CardapioItem> { sanduiche, batata, refri };
        var appService = new PedidoAppService(new PedidoRepositorioFake(), new CardapioRepositorioFake(itensCardapio));

        var itensPedido = new List<PedidoItemInputDto> 
        { 
            new() { CardapioItemId = sanduiche.Id, Quantidade = 1 } 
        };

        if (comBatata) itensPedido.Add(new PedidoItemInputDto { CardapioItemId = batata.Id, Quantidade = 1 });
        if (comRefri) itensPedido.Add(new PedidoItemInputDto { CardapioItemId = refri.Id, Quantidade = 1 });

        var resposta = appService.CriarPedido(new CriarPedidoInputDto { Itens = itensPedido });

        var subtotalEsperado = 10m + (comBatata ? 5m : 0m) + (comRefri ? 5m : 0m);
        var descontoEsperado = subtotalEsperado * percentualEsperado;

        Assert.Empty(resposta.Erros);
        Assert.Equal(subtotalEsperado, resposta.Dados.Subtotal);
        Assert.Equal(descontoEsperado, resposta.Dados.Desconto);
        Assert.Equal(subtotalEsperado - descontoEsperado, resposta.Dados.Total);
    }

    [Fact]
    public void ObterTodos_DeveRetornarListaDePedidos()
    {
        var pedido1 = new Pedido { Id = Guid.NewGuid() };
        var pedido2 = new Pedido { Id = Guid.NewGuid() };
        var appService = new PedidoAppService(
            new PedidoRepositorioFake(pedido1, pedido2),
            new CardapioRepositorioFake([]));

        var resposta = appService.ObterTodosPedidos();

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal(2, resposta.Dados.Count);
    }

    [Fact]
    public void Atualizar_DeveAtualizarPedido_QuandoDadosValidos()
    {
        var itemSanduiche = new CardapioItem { Id = Guid.NewGuid(), Nome = "Sanduiche", Preco = 10m, Categoria = CategoriaItemCardapio.Sanduiche };
        var pedido = new Pedido { Id = Guid.NewGuid() };
        var appService = new PedidoAppService(
            new PedidoRepositorioFake(pedido),
            new CardapioRepositorioFake([itemSanduiche]));

        var resposta = appService.EditarPedido(pedido.Id, new AtualizarPedidoInputDto
        {
            Itens = [new PedidoItemInputDto { CardapioItemId = itemSanduiche.Id, Quantidade = 1 }]
        });

        Assert.Empty(resposta.Erros);
        Assert.NotNull(resposta.Dados);
        Assert.Equal(10m, resposta.Dados.Total);
    }

    [Fact]
    public void Remover_DeveRemoverPedido_QuandoExiste()
    {
        var pedido = new Pedido { Id = Guid.NewGuid() };
        var appService = new PedidoAppService(
            new PedidoRepositorioFake(pedido),
            new CardapioRepositorioFake([]));

        var resposta = appService.RemoverPedido(pedido.Id);

        Assert.Empty(resposta.Erros);
        Assert.True(resposta.Dados);
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

        public IEnumerable<Pedido> ObterTodos() => _pedidos;

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
            if (pedido != null) _pedidos.Remove(pedido);
        }
    }

    private sealed class CardapioRepositorioFake(List<CardapioItem> itens) : ICardapioRepositorio
    {
        public List<CardapioItem> ObterTodos() => itens;
        public CardapioItem? ObterPorId(Guid id) => itens.FirstOrDefault(x => x.Id == id);
    }
}
