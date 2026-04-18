using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Dtos.Usuarios;

namespace Lanchonete.Application.Interfaces;

public interface IUsuarioAppService
{
    RespostaOutputDto<UsuarioOutputDto> CriarUsuario(CriarUsuarioInputDto entrada);
}
