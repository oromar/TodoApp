using System;
using System.Threading;

namespace TodoApp.Application.Queries
{
    public interface ITodoQueriesService
    {
        Task<PaginationViewModel<TodoViewModel>> ListAll(int page, int limit);
        Task<TodoViewModel> GetById(Guid id);
        Task<PaginationViewModel<TodoViewModel>> ListUncomplete(int page, int limit);
    }
}