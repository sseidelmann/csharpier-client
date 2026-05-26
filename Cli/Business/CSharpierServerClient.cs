using System.Net.Http.Json;
using Cgnd.CSharpier.Cli.Entities;
using CsharpierCodeInsights;
using CSharpierLinter.Entities;
using CSharpierLinter.Interfaces;

namespace CSharpierLinter.Business;

public class CSharpierServerClient : ICSharpierServerClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _serverUrl;

    public CSharpierServerClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _serverUrl =
            Environment.GetEnvironmentVariable("CSHARPIER_SERVER_URL")
            ?? "http://127.0.0.1:8090/format";
    }

    public async Task<FormatResponse?> FormatAsync(FormatRequest request)
    {
        using var client = _httpClientFactory.CreateClient();

        client.BaseAddress = new Uri(_serverUrl);

        var response = await client.PostAsJsonAsync("/format", request);
        return await response.Content.ReadFromJsonAsync<FormatResponse>();
    }
}
