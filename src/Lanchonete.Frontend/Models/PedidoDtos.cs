namespace Lanchonete.Frontend.Models;

public sealed class PedidoOutputDto
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public List<PedidoItemOutputDto> Itens { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
}

public sealed class PedidoItemOutputDto
{
    public Guid CardapioItemId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal SubtotalItem { get; set; }
}

public sealed class CriarPedidoInputDto
{
    public List<PedidoItemInputDto> Itens { get; set; } = [];
}

public sealed class PedidoItemInputDto
{
    public Guid CardapioItemId { get; set; }
    public int Quantidade { get; set; }
}

public sealed class CardapioItemOutputDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
}

public sealed class RespostaOutputDto<T>
{
    public T? Dados { get; set; }
    public List<string> Erros { get; set; } = [];
}
