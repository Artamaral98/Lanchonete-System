using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Lanchonete.Frontend.Infrastructure;

public sealed class JwtAuthorizationMessageHandler(ILocalStorageService localStorage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>("authToken", cancellationToken);

            if (!string.IsNullOrWhiteSpace(token))
            {
                token = token.Trim().Trim('"');
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch
        {
            // JS Interop pode não estar disponível no primeiro ciclo de renderização
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
