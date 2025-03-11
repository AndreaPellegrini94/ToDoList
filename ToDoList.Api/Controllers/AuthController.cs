using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Api.Models;
using System.Text.RegularExpressions;

namespace ToDoList.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest("Invalid login request.");
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

        if (result.Succeeded)
        {
            var token = GenerateJWTToken(user);
            return Ok(new { token });
        }
        else
        {
            return Unauthorized("Invalid password.");
        }
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest("Invalid registration request.");
        }

        if(!IsValidEmail(model.Email))
        {
            return BadRequest("Invalid email.");
        }


        var user = new User { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "Registration successful" });
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    private string GenerateJWTToken(User user)
    {
        
        var keystring = _configuration["JwtSecurityKey"];
        var key = Encoding.UTF8.GetBytes(keystring);

        var claims= new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
  
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    // Funzione per validare l'email con una Regular Expression
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Regex per email base
        return Regex.IsMatch(email, pattern);
    }
}
