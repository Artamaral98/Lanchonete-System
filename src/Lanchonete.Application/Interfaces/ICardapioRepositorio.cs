using Lanchonete.Domain.Entidades;

namespace Lanchonete.Application.Interfaces;

public interface ICardapioRepositorio
{
    List<CardapioItem> ObterTodos();
    CardapioItem? ObterPorId(Guid id);
}
