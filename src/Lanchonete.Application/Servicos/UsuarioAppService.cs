using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Dtos.Usuarios;
using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Entidades;
using Lanchonete.Domain.Exceptions;
using Lanchonete.Application.Constantes;

namespace Lanchonete.Application.Servicos;

public sealed class UsuarioAppService(
    IUsuarioRepositorio usuarioRepositorio,
    ICriptografiaServico criptografiaServico,
    IGeradorTokenServico geradorTokenServico) : IUsuarioAppService
{
    public RespostaOutputDto<UsuarioOutputDto> CriarUsuario(CriarUsuarioInputDto entrada)
    {
        var resposta = new RespostaOutputDto<UsuarioOutputDto>();

        try
        {
            var usuarioExistente = usuarioRepositorio.ObterPorLogin(entrada.Login);
            if (usuarioExistente is not null)
                throw new BusinessException("Já existe um usuário cadastrado com este login.");

            criptografiaServico.CriarHash(entrada.Senha, out var hash, out var salt);

            var usuario = new Usuario
            {
                Login = entrada.Login,
                SenhaHash = hash,
                SenhaSalt = salt
            };

            usuarioRepositorio.Criar(usuario);

            var token = geradorTokenServico.GerarToken(usuario.Login);

            resposta.Dados = new UsuarioOutputDto
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
