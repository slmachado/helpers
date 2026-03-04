using System.Net.Http.Headers;

namespace Helpers;

/// <summary>
/// A fluent wrapper over HttpClient for easier API interactions.
/// </summary>
public sealed class FluentHttpClient : IDisposable
{
    private readonly HttpClient _client;
    private string? _baseUrl;
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, string> _queryParameters = new();
    private TimeSpan? _timeout;

    public FluentHttpClient(HttpClient? client = null)
    {
        _client = client ?? new HttpClient();
    }

    public static FluentHttpClient Create() => new();

    public FluentHttpClient WithBaseUrl(string url)
    {
        _baseUrl = url.TrimEnd('/');
        return this;
    }

    public FluentHttpClient WithHeader(string name, string value)
    {
        _headers[name] = value;
        return this;
    }

    public FluentHttpClient WithQueryParam(string name, string value)
    {
        _queryParameters[name] = value;
        return this;
    }

    public FluentHttpClient WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    public async Task<T?> GetAsync<T>(string path) => await SendAsync<T>(HttpMethod.Get, path);
    public async Task<T?> PostAsync<T>(string path, object? body = null) => await SendAsync<T>(HttpMethod.Post, path, body);
    public async Task<T?> PutAsync<T>(string path, object? body = null) => await SendAsync<T>(HttpMethod.Put, path, body);
    public async Task<T?> PatchAsync<T>(string path, object? body = null) => await SendAsync<T>(HttpMethod.Patch, path, body);
    public async Task<T?> DeleteAsync<T>(string path) => await SendAsync<T>(HttpMethod.Delete, path);

    private async Task<T?> SendAsync<T>(HttpMethod method, string path, object? body = null)
    {
        var url = BuildUrl(path);
        using var request = new HttpRequestMessage(method, url);

        foreach (var header in _headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (_timeout.HasValue)
        {
            using var cts = new CancellationTokenSource(_timeout.Value);
            return await ExecuteRequest<T>(request, cts.Token);
        }

        return await ExecuteRequest<T>(request, CancellationToken.None);
    }

    private async Task<T?> ExecuteRequest<T>(HttpRequestMessage request, CancellationToken token)
    {
        var response = await _client.SendAsync(request, token);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(token);
            throw new HttpRequestException($"Request to {request.RequestUri} failed with status {response.StatusCode}. Content: {errorContent}", null, response.StatusCode);
        }

        var jsonResponse = await response.Content.ReadAsStringAsync(token);
        return string.IsNullOrWhiteSpace(jsonResponse) ? default : JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private string BuildUrl(string path)
    {
        var fullUrl = _baseUrl != null ? $"{_baseUrl}/{path.TrimStart('/')}" : path;
        
        if (_queryParameters.Count > 0)
        {
            var query = string.Join("&", _queryParameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            fullUrl += (fullUrl.Contains('?') ? "&" : "?") + query;
        }

        return fullUrl;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
