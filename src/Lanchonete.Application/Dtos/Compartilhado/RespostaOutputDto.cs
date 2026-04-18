namespace Lanchonete.Application.Dtos.Compartilhado;

public sealed class RespostaOutputDto<T>
{
    public T? Dados { get; set; }
    public List<string> Erros { get; set; } = [];
}
