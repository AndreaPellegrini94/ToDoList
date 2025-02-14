using System.Net.Http.Json;
using System.Text.Json;
using ToDoList.Api.Models;


public class ToDoService
{
    private readonly HttpClient _http;
    public ToDoService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ToDoItem>> GetToDosAsync()
    {
        var result = await _http.GetFromJsonAsync<List<ToDoItem>>("https://localhost:7264/api/ToDo");
        return result;

    }


    public async Task AddToDoAsync(string title) 
    {
        var newToDo = new ToDoItem { Title = title };
        var url = "https://localhost:7264/api/ToDo/Add";

        var response = await _http.PostAsJsonAsync( url, newToDo);
        
    }


    public async Task UpdateToDoAsync(ToDoItem todo)
    {
        var url = $"https://localhost:7264/api/ToDo/Update/{todo.Id}";
        await _http.PutAsJsonAsync(url, todo);
    }

    public async Task DeleteToDoAsync(int id)
    {
        var url = $"https://localhost:7264/api/ToDo/Delete/{id}";
        await _http.DeleteAsync(url);
    }
       
}

