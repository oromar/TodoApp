using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoApp.Infra.Context
{
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseSqlServer("Server=NB411-OLDM\\SQLEXPRESS;Database=TODO-APP;Trusted_Connection=True;MultipleActiveResultSets=True;");
            return new TodoContext(optionsBuilder.Options);
        }
    }
}
