using DatabaseLibrary.Models;
using System.Net;
using System.Text.Json;

public class FilmServiceStatusHandling
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public FilmServiceStatusHandling(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://localhost:7000/api/");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<Film> GetFilmByIdAsync(int id)
    {
        var response = await _client.GetAsync($"films/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Film>(content, _jsonOptions);
    }

    public async Task<List<Film>> GetFilmsWithStatusHandlingAsync()
    {
        var response = await _client.GetAsync("films");

        return response.StatusCode switch
        {
            HttpStatusCode.OK =>
                JsonSerializer.Deserialize<List<Film>>(
                    await response.Content.ReadAsStringAsync(), _jsonOptions),
            HttpStatusCode.NoContent => new List<Film>(),
            HttpStatusCode.NotFound => throw new Exception("Ресурс не найден"),
            _ when !response.IsSuccessStatusCode =>
                throw new HttpRequestException($"Error: {response.StatusCode}"),
            _ => new List<Film>()
        };
    }
}