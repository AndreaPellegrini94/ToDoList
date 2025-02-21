using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Models;

namespace ToDoList.Api.Data
{
    public class ToDodbContext : IdentityDbContext<User>  
    {
        public ToDodbContext(DbContextOptions<ToDodbContext> options) : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }

    
    }
}
