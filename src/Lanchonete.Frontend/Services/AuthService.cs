using System.Net.Http.Json;
using Blazored.LocalStorage;
using Lanchonete.Frontend.Infrastructure;
using Lanchonete.Frontend.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Lanchonete.Frontend.Services;

public sealed class AuthService(
    HttpClient httpClient, 
    ILocalStorageService localStorage, 
    AuthenticationStateProvider authStateProvider)
{
    public async Task<RespostaOutputDto<LoginOutputDto>> Login(LoginInputDto input)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/Autenticacao/Login", input);
        var result = await response.Content.ReadFromJsonAsync<RespostaOutputDto<LoginOutputDto>>();

        if (response.IsSuccessStatusCode && result?.Dados != null)
        {
            await localStorage.SetItemAsync("authToken", result.Dados.Token);
            ((CustomAuthenticationStateProvider)authStateProvider).NotifyUserAuthentication(result.Dados.Token);
        }

        return result ?? new RespostaOutputDto<LoginOutputDto> { Erros = ["Erro ao processar resposta"] };
    }

    public async Task<RespostaOutputDto<UsuarioOutputDto>> Register(CriarUsuarioInputDto input)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/Usuarios", input);
        var result = await response.Content.ReadFromJsonAsync<RespostaOutputDto<UsuarioOutputDto>>();

        if (response.IsSuccessStatusCode && result?.Dados != null)
        {
            await localStorage.SetItemAsync("authToken", result.Dados.Token);
            ((CustomAuthenticationStateProvider)authStateProvider).NotifyUserAuthentication(result.Dados.Token);
        }

        return result ?? new RespostaOutputDto<UsuarioOutputDto> { Erros = ["Erro ao processar resposta"] };
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider)authStateProvider).NotifyUserLogout();
    }
}
