using Lanchonete.Application.Dtos.Cardapio;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class CardapioController(ICardapioAppService cardapioAppService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ObterCardapio()
    {
        var resultado = cardapioAppService.ObterCardapio();
        return Ok(resultado);
    }
}
