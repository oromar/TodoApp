using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Data
{
    public interface ITodoRepository
    {
        Task<Todo> Add(Todo todo);
        Task<Todo> Update(Todo todo);
        Task Remove(Guid id);
        Task<Todo> Get(Guid id);
        Task<List<Todo>> List(Expression<Func<Todo, bool>> predicate = null);
    }
}
