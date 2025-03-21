using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToDoList.Api.Models;

public class Validation
{
    // Funzione per validare l'email con una Regular Expression
    public class ValidEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            string email = value.ToString();
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Regex per email

            return Regex.IsMatch(email, pattern);
        }
    }
}
