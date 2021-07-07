using MediatR;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;

namespace TodoApp.Application.Queries
{
    public class GetListTodosQuery : PaginationQueryBase, IRequest<PaginationViewModel<TodoViewModel>>
    {

        public GetListTodosQuery(int page, int limit) : base(page, limit)
        {
        }
    }
}
