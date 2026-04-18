using System.Net.Http.Json;
using Lanchonete.Frontend.Models;

namespace Lanchonete.Frontend.Services;

public sealed class CardapioService(HttpClient httpClient)
{
    public async Task<RespostaOutputDto<List<CardapioItemOutputDto>>> GetItens()
    {
        return await httpClient.GetFromJsonAsync<RespostaOutputDto<List<CardapioItemOutputDto>>>("api/v1/Cardapio") 
               ?? new RespostaOutputDto<List<CardapioItemOutputDto>>();
    }
}

public sealed class PedidoService(HttpClient httpClient)
{
    public async Task<RespostaOutputDto<List<PedidoOutputDto>>> GetPedidos()
    {
        return await httpClient.GetFromJsonAsync<RespostaOutputDto<List<PedidoOutputDto>>>("api/v1/Pedidos")
               ?? new RespostaOutputDto<List<PedidoOutputDto>>();
    }

    public async Task<RespostaOutputDto<PedidoOutputDto>> CriarPedido(CriarPedidoInputDto input)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/Pedidos", input);
        return await response.Content.ReadFromJsonAsync<RespostaOutputDto<PedidoOutputDto>>()
               ?? new RespostaOutputDto<PedidoOutputDto>();
    }

    public async Task<RespostaOutputDto<PedidoOutputDto>> AtualizarPedido(Guid id, CriarPedidoInputDto input)
    {
        var response = await httpClient.PutAsJsonAsync($"api/v1/Pedidos/{id}", input);
        return await response.Content.ReadFromJsonAsync<RespostaOutputDto<PedidoOutputDto>>()
               ?? new RespostaOutputDto<PedidoOutputDto>();
    }

    public async Task<bool> RemoverPedido(Guid id)
    {
        var response = await httpClient.DeleteAsync($"api/v1/Pedidos/{id}");
        return response.IsSuccessStatusCode;
    }
}
