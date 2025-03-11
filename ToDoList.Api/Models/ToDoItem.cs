namespace ToDoList.Api.Models;
public class ToDoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; } = false;

    public string UserId { get; set; }

    //public User User { get; set; } per il momento non serve, complica solo il json (le restanti info sullo user posso ricavarle dal jwt)
}
