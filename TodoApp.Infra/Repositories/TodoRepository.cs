using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoApp.Domain.Data;
using TodoApp.Domain.Entities;
using TodoApp.Infra.Context;

namespace TodoApp.Infra.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext context;

        public TodoRepository(TodoContext context)
        {
            this.context = context;
        }

        public async Task<Todo> Add(Todo todo)
        {
            await context.AddAsync(todo);
            await context.SaveChangesAsync();
            return todo;
        }

        public async Task<List<Todo>> List(Expression<Func<Todo, bool>> predicate = null)
        {
            if (predicate != null)
                return await context.Todos.Where(predicate).ToListAsync();
            return await context.Todos.ToListAsync();
        }

        public async Task<Todo> Get(Guid id)
        {
            return await context.Todos.FirstOrDefaultAsync(a => a.Id == id);
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
