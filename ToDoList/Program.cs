using ToDoList.Components;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi i servizi per le sessioni
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Aggiungi i servizi al container
builder.Services.AddScoped<ToDoService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<AuthService>("ToDoList.Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7264");
});

var app = builder.Build();

// Configura il middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Abilita le sessioni
app.UseSession();

// Aggiungi il token alla richiesta se esiste in sessione
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
