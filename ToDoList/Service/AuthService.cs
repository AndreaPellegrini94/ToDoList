
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDoList.Api.Models;
using Microsoft.JSInterop;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private string _currentToken;
    private bool _tokenLoaded = false;

    public string CurrentToken => _currentToken;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<Result> RegisterAsync(string username, string email, string password)
    {
        Register registerModel = new();
        registerModel.UserName = username;
        registerModel.Email = email;
        registerModel.Password = password;
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerModel);

        if (response.IsSuccessStatusCode)
        {
            return new Result { IsSuccess = true };
        }
        else
        {
            // Assumi che il messaggio di errore venga restituito nel corpo della risposta
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new Result { IsSuccess = false, ErrorMessage = errorMessage };
        }
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var loginData = new { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _currentToken = result.Token;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);

            return result.Token;
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new Exception("Login fallito: " + errorMessage);
    }

    public async Task<string> GetTokenAsync()
    {
        if (!_tokenLoaded)
        {
            _currentToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            _tokenLoaded = true; // Evita richiami multipli non necessari
        }
        return _currentToken;
    }
    public async Task<string> GetUserIdAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return null; // o gestisci l'errore come preferisci
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assumendo che l'ID utente sia memorizzato nel claim "nameid"
        var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        Console.WriteLine("UserId: " + userId);

        return userId;
    }
    public async Task LogoutAsync()
    {
        _currentToken = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}

