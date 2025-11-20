using DatabaseLibrary.Models;
using System.Net.Http.Json;
public class FilmServiceAuto
{
    private readonly HttpClient _client;

    public FilmServiceAuto(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("http://localhost:5070/api/");
    }
    public async Task<List<Film>> GetFilmsAsync()
    {
        return await _client.GetFromJsonAsync<List<Film>>("films");
    }

    public async Task<Film> GetFilmsByIdAsync(int id)
    {
        var response = await _client.GetAsync($"films/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Film>();
    }

    public async Task<Film> CreateFilmAsync(Film film)
    {
        var response = await _client.PostAsJsonAsync("films", film);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Film>();
    }

    public async Task<Film> UpdateFilmAsync(int id, Film film)
    {
        var response = await _client.PutAsJsonAsync($"films/{id}", film);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Film>();
    }

    public async Task<bool> DeleteFilmAsync(int id)
    {
        var response = await _client.DeleteAsync($"category/{id}");
        response.EnsureSuccessStatusCode();
        return true;
    }
}