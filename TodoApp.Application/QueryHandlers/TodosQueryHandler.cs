using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Common;
using TodoApp.Application.Queries;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;
using TodoApp.Domain.Data;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.CommandHandlers
{
    public class TodosQueryHandler :
        IRequestHandler<GetListTodosQuery, PaginationViewModel<TodoViewModel>>,
        IRequestHandler<GetTodoByIdQuery, TodoViewModel>,
        IRequestHandler<GetUncompletedTodosQuery, PaginationViewModel<TodoViewModel>>
    {
        private readonly ITodoRepository repository;

        public TodosQueryHandler(ITodoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PaginationViewModel<TodoViewModel>> Handle(GetListTodosQuery request, CancellationToken cancellationToken)
        {
            var total = repository.AsQueryable().Count();
            var todos = repository.AsQueryable().OrderBy(a => a.CreationDate).Skip(request.Offset).Take(request.Limit).ToList();
            var items = todos.Select(a => new TodoViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Completed = a.Completed,
                CompletedDate = a.Completed ? a.CompletedDate : (DateTime?)null
            })
            .ToList();
            return new PaginationViewModel<TodoViewModel>(items, total, request.Page, request.Limit);
        }

        public async Task<TodoViewModel> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Todo, bool>> predicate = a => a.Id == request.Id;
            var todo = repository.AsQueryable().FirstOrDefault(predicate);
            if (todo != null)
                return new TodoViewModel
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Completed = todo.Completed,
                    CompletedDate = todo.Completed ? todo.CompletedDate : (DateTime?)null
                };
            throw new TodoAppException("Todo not found.");
        }

        public async Task<PaginationViewModel<TodoViewModel>> Handle(GetUncompletedTodosQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Todo, bool>> predicate = a => !a.Completed;
            var total = repository.AsQueryable().Count(predicate);
            var todos = repository.AsQueryable().Where(predicate).OrderBy(a => a.CreationDate).Skip(request.Offset).Take(request.Limit).ToList();
            var items = todos.Select(a => new TodoViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
            })
            .ToList();
            return new PaginationViewModel<TodoViewModel>(items, total, request.Page, request.Limit);
        }
    }
}
