using Lanchonete.Application.Dtos.Autenticacao;
using Lanchonete.Application.Dtos.Compartilhado;

namespace Lanchonete.Application.Interfaces;

public interface IAutenticacaoAppService
{
    RespostaOutputDto<LoginOutputDto> RealizarLogin(LoginInputDto entrada);
}
