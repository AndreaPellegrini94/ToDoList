using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data;
using ToDoList.Api.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;



namespace ToDoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class ToDoController : ControllerBase
{
    private readonly ToDodbContext _context;

    public ToDoController(ToDodbContext context) => _context = context;

    
    [HttpGet]//mi preleva la lista dei todo
    public async Task<IActionResult> GetToDos()
    {   
        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//dovrebbe darmi l'identificativo dell'utente
        //System.Console.WriteLine($"{UserID}");
        if (UserID == null) return Unauthorized("User ID not found");

        var todos = await _context.ToDoItems
            .Where(x => x.UserId == UserID)
            .ToListAsync();

        return Ok(todos);
    }

    
    [HttpPost("Add")]//mi aggiunge un todo
    public async Task<IActionResult> AddToDo(ToDoItem todo)
    {
        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        todo.UserId = UserID;//associamo il todo all'utente loggato

        _context.ToDoItems.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetToDos), new { id = todo.Id }, todo);
    }

    [HttpPut("Update/{id}")]//mi permette la modifica di un todo esistente (lato frontend vorrei poter modificare sia lo stato che il nome dell' attivitá)
    public async Task<IActionResult> UpdateToDo(int id, ToDoItem todo)
    {
        var existing = await _context.ToDoItems.FindAsync(id);
        if (existing == null) return NotFound("The selected element does not exist");///non mi piace molto, ma dá un 404 quando non esiste id, ma fino ad oggi l'ho visto cosí 

        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (UserID == null) return Unauthorized("You are not Utorized to modify this list");

        existing.Title = todo.Title;
        existing.IsDone = todo.IsDone;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteToDo(int id)
    {
        var existing = await _context.ToDoItems.FindAsync(id);
        if (existing == null) return NotFound("The selected element does not exist");

        
        var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (UserID == null) return Unauthorized("You are not Utorized to delete this list");

        _context.ToDoItems.Remove(existing);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
