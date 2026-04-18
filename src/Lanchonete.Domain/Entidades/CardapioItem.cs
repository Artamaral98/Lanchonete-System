using Lanchonete.Domain.Enums;

namespace Lanchonete.Domain.Entidades;

public sealed class CardapioItem : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public CategoriaItemCardapio Categoria { get; set; }
}
