using Microsoft.EntityFrameworkCore;
using System;
using TodoApp.Infra.Context;

namespace TodoApp.Tests.Common
{
    public static class ContextFactory
    {
        public static TodoContext New()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new TodoContext(optionsBuilder.Options);
        }
    }
}
