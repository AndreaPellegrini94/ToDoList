using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoList.Api.Models;

public class ToDoService
{
    private readonly HttpClient _http;

    private string _currentToken;

    private readonly AuthService _authService;

    public ToDoService(IHttpClientFactory http, AuthService authService)
    {
        _http = http.CreateClient("ToDoList.Api");
        _authService = authService ?? throw new ArgumentNullException(nameof(authService)); // Verifica se authService è null
    }

    // Metodo per impostare il token nell'header
    public void SetAuthorizationToken(string token)
    {
        _currentToken = token;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<ToDoItem>> GetToDosAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/ToDo");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _currentToken);

        var response = await _http.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<ToDoItem>>();
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"); // Debug: mostra il codice di stato e il contenuto dell'errore
            return new List<ToDoItem>();
        }
    }


    public async Task AddToDoAsync(string title)
    {   

        var userId = await _authService.GetUserIdAsync();
        var newToDo = new ToDoItem
        {
            Title = title,
            UserId = userId // Imposta l'UserId qui
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/ToDo/Add")
        {
            Content = JsonContent.Create(newToDo)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _currentToken);

        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {content}");
        }
    }


    public async Task UpdateToDoAsync(ToDoItem todo)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/ToDo/Update/{todo.Id}")
        {
            Content = JsonContent.Create(todo)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _currentToken);

        await _http.SendAsync(request);
    }

    public async Task DeleteToDoAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/ToDo/Delete/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _currentToken);

        await _http.SendAsync(request);
    }

}
