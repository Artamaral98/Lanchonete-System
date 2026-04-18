using Asp.Versioning;
using Lanchonete.Application.Dtos.Usuarios;
using Lanchonete.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class UsuariosController(IUsuarioAppService usuarioAppService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Criar([FromBody] CriarUsuarioInputDto entrada)
    {
        var resultado = usuarioAppService.CriarUsuario(entrada);
        if (resultado.Erros.Count > 0)
        {
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }
}
