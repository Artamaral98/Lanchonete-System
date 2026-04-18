using Lanchonete.Application.Constantes;
using Lanchonete.Application.Dtos.Autenticacao;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Exceptions;

namespace Lanchonete.Application.Servicos;

public class AutenticacaoAppService(
    IValidadorCredencialServico validadorCredencialServico,
    IGeradorTokenServico geradorTokenServico) : IAutenticacaoAppService
{
    public RespostaOutputDto<LoginOutputDto> GerarToken(LoginInputDto entrada)
    {
        var resposta = new RespostaOutputDto<LoginOutputDto>();

        try
        {
            if (!validadorCredencialServico.CredencialValida(entrada.Login, entrada.Senha))
            {
                throw new BusinessException(Messages.CredenciaisInvalidas);
            }

            var token = geradorTokenServico.GerarToken(entrada.Login);
            resposta.Dados = new LoginOutputDto
            {
                Token = token
            };
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }
}
