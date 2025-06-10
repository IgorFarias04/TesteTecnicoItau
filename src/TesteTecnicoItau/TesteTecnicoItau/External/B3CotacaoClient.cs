using System.Text.Json;

public class B3CotacaoClient
{
    private readonly HttpClient _http;

    public B3CotacaoClient()
    {
        _http = new HttpClient();
    }

    public async Task<decimal?> ObterCotacaoAsync(string codigoAtivo)
    {
        try
        {
            var url = $"https://b3api.vercel.app/api/quote/{codigoAtivo}";
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            var priceString = doc.RootElement.GetProperty("price").GetRawText();
            if (decimal.TryParse(priceString, out var preco))
                return preco;

            return null;
        }
        catch
        {
            return null;
        }
    }
}