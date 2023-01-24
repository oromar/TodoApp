using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Data
{
    public interface ITodoRepository : IRepositoryBase<Todo> { }
    public interface ITodoQuery : IQueryBase<Todo> { }
}
