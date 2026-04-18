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
            if (!validadorCredencialServico.CredencialValida(entrada.Usuario, entrada.Senha))
            {
                throw new BusinessException(Messages.CredenciaisInvalidas);
            }

            var (token, expiraEmUtc) = geradorTokenServico.GerarToken(entrada.Usuario);
            resposta.Dados = new LoginOutputDto
            {
                Token = token,
                ExpiraEmUtc = expiraEmUtc
            };
        }
        catch (BusinessException ex)
        {
            resposta.Erros.Add(ex.Message);
        }

        return resposta;
    }
}
