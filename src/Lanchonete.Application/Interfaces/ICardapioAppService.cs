using Lanchonete.Application.Dtos.Cardapio;
using Lanchonete.Application.Dtos.Compartilhado;

namespace Lanchonete.Application.Interfaces;

public interface ICardapioAppService
{
    RespostaOutputDto<List<CardapioItemOutputDto>> ObterCardapio();
}
