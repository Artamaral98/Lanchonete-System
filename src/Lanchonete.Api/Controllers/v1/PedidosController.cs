using Lanchonete.Application.Constantes;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Application.Dtos.Pedidos;
using Lanchonete.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Api.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public sealed class PedidosController(IPedidoAppService pedidoAppService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Criar([FromBody] CriarPedidoInputDto entrada)
    {
        var resultado = pedidoAppService.CriarPedido(entrada);
        if (resultado.Erros.Count > 0)
        {
            return BadRequest(resultado);
        }

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Dados?.Id }, resultado);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ObterTodos()
    {
        var resultado = pedidoAppService.ObterTodosPedidos();
        return Ok(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObterPorId([FromRoute] Guid id)
    {
        var resultado = pedidoAppService.ObterPedidoPorId(id);
        if (resultado.Erros.Count > 0)
        {
            if (resultado.Erros.Contains(Messages.PedidoNaoEncontrado))
            {
                return NotFound(resultado);
            }
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Atualizar([FromRoute] Guid id, [FromBody] AtualizarPedidoInputDto entrada)
    {
        var resultado = pedidoAppService.EditarPedido(id, entrada);
        if (resultado.Erros.Count > 0)
        {
            if (resultado.Erros.Contains(Messages.PedidoNaoEncontrado))
            {
                return NotFound(resultado);
            }
            return BadRequest(resultado);
        }

        return Ok(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Remover([FromRoute] Guid id)
    {
        var resultado = pedidoAppService.RemoverPedido(id);
        if (resultado.Erros.Count > 0)
        {
            if (resultado.Erros.Contains(Messages.PedidoNaoEncontrado))
            {
                return NotFound(resultado);
            }
            return BadRequest(resultado);
        }

        return NoContent();
    }
}
