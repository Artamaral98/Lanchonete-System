namespace Lanchonete.Application.Dtos.Cardapio;

public sealed class CardapioItemOutputDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
}
