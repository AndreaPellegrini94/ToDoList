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

        if (UserID == null)
        {
            return Unauthorized("User ID not found");
        }

        var user = await _context.Users
            .Where(x => x.Id == UserID)
            .Select(x => new
            {
                x.Email
            })
            .FirstOrDefaultAsync();


        return Ok(user);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdatePassword model)
    {
        if (model == null || string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
        {
            return BadRequest("Invalid update request. Both current and new password are required.");
        }

        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (UserID == null)
        {
            return Unauthorized("You are not authorized to modify this user.");
        }

        var user = await _userManager.FindByIdAsync(UserID);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // **Gestione cambio password**
        if (string.IsNullOrEmpty(model.NewPassword))
        {
            return BadRequest("New password cannot be empty.");
        }

        // Verifica che la password corrente sia corretta
        var checkPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
        if (!checkPassword)
        {
            return BadRequest("Current password is incorrect.");
        }

        // Verifica che la nuova password sia diversa dalla corrente
        if (model.CurrentPassword == model.NewPassword)
        {
            return BadRequest("New password must be different from the current password.");
        }

        // Verifica la complessità della nuova password
        var passwordValidationResult = await _userManager.PasswordValidators.First().ValidateAsync(_userManager, user, model.NewPassword);
        if (!passwordValidationResult.Succeeded)
        {
            return BadRequest(passwordValidationResult.Errors.Select(e => e.Description));
        }

        // Cambia la password
        var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!passwordResult.Succeeded)
        {
            return BadRequest(passwordResult.Errors.Select(e => e.Description));
        }

        return Ok("Update successful");
    }



    [HttpDelete("Delete")]
    public async Task<IActionResult> DeletUsert()
    {
        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (UserID == null)
        {
            return Unauthorized("You are not Utorized do modify this User");
        }
        var user = await _userManager.FindByIdAsync(UserID);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return Ok("Delete sucesssful");
        }
        else
        {
            return BadRequest(result.Errors);
        }

    }
}
