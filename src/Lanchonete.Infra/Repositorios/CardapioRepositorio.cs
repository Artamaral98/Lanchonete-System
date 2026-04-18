using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Entidades;
using Lanchonete.Domain.Enums;

namespace Lanchonete.Infra.Repositorios;

public sealed class CardapioRepositorio : ICardapioRepositorio
{
    private static readonly CardapioItem[] ItensCardapio =
    [
        new() { Nome = "X Burger", Preco = 5.00m, Categoria = CategoriaItemCardapio.Sanduiche },
        new() { Nome = "X Egg", Preco = 4.50m, Categoria = CategoriaItemCardapio.Sanduiche },
        new() { Nome = "X Bacon", Preco = 7.00m, Categoria = CategoriaItemCardapio.Sanduiche },
        new() { Nome = "Batata frita", Preco = 2.00m, Categoria = CategoriaItemCardapio.Batata },
        new() { Nome = "Refrigerante", Preco = 2.50m, Categoria = CategoriaItemCardapio.Bebida }
    ];

    public List<CardapioItem> ObterTodos() => [.. ItensCardapio];

    public CardapioItem? ObterPorId(Guid id) => ItensCardapio.FirstOrDefault(x => x.Id == id);
}
