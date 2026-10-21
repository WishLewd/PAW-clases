using PAW.Architecture.Providers;
using System.Text.Json.Serialization;

namespace PAW.Web.Services;

public interface IExchangeRateService
{
    Task<TipoCambioResponse?> GetExchangeRateAsync(CancellationToken ct = default);
}

public class ExchangeRateService : IExchangeRateService
{
    private readonly TipoCambioClient _client;

    public ExchangeRateService(TipoCambioClient? client = null)
    {
        _client = client ?? new TipoCambioClient();
    }

    public async Task<TipoCambioResponse?> GetExchangeRateAsync(CancellationToken ct = default)
    {
        try
        {
            return await _client.GetTipoCambioAsync(ct);
        }
        catch
        {
            return null;
        }
    }
}

// DTOs for the API response
public class TipoCambioResponse
{
    [JsonPropertyName("dolar")]
    public Dolar Dolar { get; set; } = new();

    [JsonPropertyName("euro")]
    public Euro Euro { get; set; } = new();
}

public class Dolar
{
    [JsonPropertyName("venta")]
    public ValorFecha Venta { get; set; } = new();

    [JsonPropertyName("compra")]
    public ValorFecha Compra { get; set; } = new();
}

public class ValorFecha
{
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }
}

public class Euro
{
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("dolares")]
    public decimal Dolares { get; set; }

    [JsonPropertyName("colones")]
    public decimal Colones { get; set; }
}

public class TipoCambioClient
{
    private const string Url = "https://api.hacienda.go.cr/indicadores/tc";
    private readonly HttpClient _http;

    public TipoCambioClient(HttpClient? http = null)
    {
        _http = http ?? new HttpClient();
    }

    public async Task<TipoCambioResponse?> GetTipoCambioAsync(CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, Url)
        {
            Version = System.Net.HttpVersion.Version11,
            VersionPolicy = HttpVersionPolicy.RequestVersionExact
        };
        req.Headers.UserAgent.ParseAdd("curl/8.7.1");
        req.Headers.Accept.ParseAdd("*/*");

        var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await res.Content.ReadAsStringAsync(ct);
        return await JsonProvider.DeserializeAsync<TipoCambioResponse>(content);
    }
}
