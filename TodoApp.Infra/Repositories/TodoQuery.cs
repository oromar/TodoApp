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
    public class TodoQuery : ITodoQuery
    {
        protected readonly TodoContext context;

        public TodoQuery(TodoContext context)
        {
            this.context = context;
        }

        public IQueryable<Todo> AsQueryable()
        {
            return context.Todos.AsQueryable();
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
    }
}
