using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Models;

namespace ToDoList.Api.Data
{
    public class ToDodbContext : DbContext
    {
        public ToDodbContext(DbContextOptions<ToDodbContext> options) : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; } = null!;

    }
}
