using static ToDoList.Api.Models.Validation;

namespace ToDoList.Api.Models;


public class UpdateUser
{
    [ValidEmail]
    public string Email { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
