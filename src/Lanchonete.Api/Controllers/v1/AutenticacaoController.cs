using Asp.Versioning;
using Lanchonete.Application.Dtos.Autenticacao;
using Lanchonete.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/[controller]/[action]")]
public sealed class AutenticacaoController(IAutenticacaoAppService autenticacaoAppService) : ControllerBase
{
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] LoginInputDto entrada)
    {
        var resultado = autenticacaoAppService.GerarToken(entrada);
        if (resultado.Erros.Count > 0)
        {
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }
}
