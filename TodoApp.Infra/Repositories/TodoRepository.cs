using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoApp.Domain.Data;
using TodoApp.Domain.Entities;
using TodoApp.Infra.Context;

namespace TodoApp.Infra.Repositories
{
    public class TodoRepository : TodoQuery, ITodoRepository
    {
        public TodoRepository(TodoContext context) : base(context)
        {

        }

        public async Task<Todo> Add(Todo todo)
        {
            await context.AddAsync(todo);
            await context.SaveChangesAsync();
            return todo;
        }

        public async Task Remove(Guid id)
        {
            var itemToRemove = await context.Todos.FirstOrDefaultAsync(a => a.Id == id);
            if (itemToRemove != null)
            {
                context.Todos.Remove(itemToRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Todo> Update(Todo todo)
        {
            context.Todos.Update(todo);
            await context.SaveChangesAsync();
            return todo;
        }
    }
}
