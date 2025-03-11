using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data;
using Microsoft.AspNetCore.Identity;
using ToDoList.Api.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ToDodbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ToDodbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, //se non ho issuer e audience, non posso verificarli, quindi li metto false
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])),
        ValidIssuer = null, // Sostituisci con il tuo issuer
        ValidAudience = null// Sostituisci con il tuo audience
    };
});

builder.Services.ConfigureApplicationCookie(c =>
{
    c.Events.OnRedirectToLogin
        = ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                ctx.Response.StatusCode = 401;

            return Task.CompletedTask;
        };
    c.Events.OnRedirectToAccessDenied
        = ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                ctx.Response.StatusCode = 403;

            return Task.CompletedTask;
        };
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
     {
         new OpenApiSecurityScheme
            {
                 Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
            },
                Array.Empty<string>()
     }
     });
});

builder.Logging.AddConsole();  // provo a debuggare tramite console



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//necessario per poter usare i cors
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
