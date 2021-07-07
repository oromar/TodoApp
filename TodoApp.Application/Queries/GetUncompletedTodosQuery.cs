using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;

namespace TodoApp.Application.Queries
{
    public class GetUncompletedTodosQuery : PaginationQueryBase, IRequest<PaginationViewModel<TodoViewModel>>
    {

        public GetUncompletedTodosQuery(int page, int limit) : base(page, limit)
        {
        }
    }
}

