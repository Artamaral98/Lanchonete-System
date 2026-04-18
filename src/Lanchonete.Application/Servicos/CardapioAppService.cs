using Lanchonete.Application.Dtos.Cardapio;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Interfaces;

namespace Lanchonete.Application.Servicos;

public class CardapioAppService(ICardapioRepositorio cardapioRepositorio) : ICardapioAppService
{
    public RespostaOutputDto<List<CardapioItemOutputDto>> ObterCardapio()
    {
        var resposta = new RespostaOutputDto<List<CardapioItemOutputDto>>();
        var itens = cardapioRepositorio.ObterTodos();

        resposta.Dados = itens
            .Select(x => new CardapioItemOutputDto
            {
                Id = x.Id,
                Nome = x.Nome,
                Preco = x.Preco,
                Categoria = x.Categoria.ToString()
            })
            .ToList();

        return resposta;
    }
}
