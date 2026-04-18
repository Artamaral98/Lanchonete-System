using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Lanchonete.Frontend.Infrastructure;

public sealed class JwtAuthorizationMessageHandler(ILocalStorageService localStorage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await localStorage.GetItemAsync<string>("authToken", cancellationToken);

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
