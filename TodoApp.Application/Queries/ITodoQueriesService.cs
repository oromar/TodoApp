using System;
using System.Threading.Tasks;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;

namespace TodoApp.Application.Queries
{
    public interface ITodoQueriesService
    {
        Task<PaginationViewModel<TodoViewModel>> ListAll(int page, int limit);
        Task<TodoViewModel> GetById(Guid id);
        Task<PaginationViewModel<TodoViewModel>> ListUncomplete(int page, int limit);
        Task<PaginationViewModel<TodoViewModel>> Search(string criteria, int page, int limit);
    }
}