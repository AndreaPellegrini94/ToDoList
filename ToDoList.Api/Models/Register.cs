﻿using static ToDoList.Api.Models.Validation;

namespace ToDoList.Api.Models;

public class Register
{
    public string UserName { get; set; }
    [ValidEmail]
    public string Email { get; set; }
    public string Password { get; set; }

}
