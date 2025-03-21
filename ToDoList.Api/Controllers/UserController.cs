using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data;
using ToDoList.Api.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace ToDoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class UserController : Controller
{
    private readonly ToDodbContext _context;
    private readonly UserManager<User> _userManager;

    public UserController(ToDodbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]//post login mi permette di andare  aprendere i dati del singolo user loggato
    public async Task<IActionResult> GetUser()
    {
        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//dovrebbe darmi l'identificativo dell'utente

        if (UserID == null) return Unauthorized("User ID not found");

        var user = await _context.Users
            .Where(x => x.Id == UserID)
            .FirstOrDefaultAsync();


        return Ok(user);
    }

    [HttpPut("Update")]//mi permette di modificare i dati dell'utente loggato
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUser model)
    {
        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (UserID == null) return Unauthorized("You are not Utorized do modify this User");

        var user = await _userManager.FindByIdAsync(UserID);

        if (user == null) return NotFound("User not found");

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid email.");
        }

        var errors = new List<string>();

        if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                errors.Add("Email already in use.");
            }
            else
            {
                user.Email = model.Email;
            }
        }

        // **Gestione cambio password**
        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            if (string.IsNullOrEmpty(model.CurrentPassword))
            {
                errors.Add("Current password is required to change the password.");
            }
            else
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!checkPassword)
                {
                    errors.Add("Current password is incorrect.");
                }
                else
                {
                    var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!passwordResult.Succeeded)
                    {
                        errors.AddRange(passwordResult.Errors.Select(e => e.Description));
                    }
                }
            }
        }

        if (errors.Any())
        {
            return BadRequest(errors);
        }

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok("Updete sucesssful");
        }
        else
        {
            return BadRequest(result.Errors);
        }

    }
}
