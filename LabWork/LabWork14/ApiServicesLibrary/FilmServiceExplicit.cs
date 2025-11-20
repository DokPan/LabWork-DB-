using DatabaseLibrary.Models;
using System.Text;
using System.Text.Json;
public class FilmServiceExplicit
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public FilmServiceExplicit(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://localhost:7000/api/");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<List<Film>> GetFilmsAsync()
    {
        var response = await _client.GetAsync("films");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Film>>(content, _jsonOptions);
    }

    public async Task<Film> GetFilmByIdAsync(int id)
    {
        var response = await _client.GetAsync($"films/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Film>(content, _jsonOptions);
    }

    public async Task<Film> CreateFilmAsync(Film film)
    {
        var json = JsonSerializer.Serialize(film, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("films", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Film>(responseContent, _jsonOptions);
    }

    public async Task<Film> UpdateFilmAsync(int id, Film film)
    {
        var json = JsonSerializer.Serialize(film, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"films/{id}", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Film>(responseContent, _jsonOptions);
    }

    public async Task<bool> DeleteFilmAsync(int id)
    {
        var response = await _client.DeleteAsync($"films/{id}");
        response.EnsureSuccessStatusCode();
        return true;
    }
}