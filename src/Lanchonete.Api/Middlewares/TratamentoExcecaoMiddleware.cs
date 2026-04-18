using System.Net;
using System.Text.Json;
using Lanchonete.Application.Constantes;
using Lanchonete.Application.Dtos.Compartilhado;
using Lanchonete.Domain.Exceptions;

namespace Lanchonete.Api.Middlewares;

public sealed class TratamentoExcecaoMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await TratarExcecaoAsync(context, ex);
        }
    }

    private static async Task TratarExcecaoAsync(HttpContext context, Exception exception)
    {
        var resposta = new RespostaOutputDto<object>();
        var statusCode = exception switch
        {
            BusinessException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            InvalidOperationException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        resposta.Erros.Add(exception is BusinessException ? exception.Message : Messages.ErroInesperado);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(resposta);
        await context.Response.WriteAsync(json);
    }
}
