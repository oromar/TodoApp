using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Commands;
using TodoApp.Application.Common;
using TodoApp.Domain;
using TodoApp.Domain.Data;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.CommandHandlers
{
    public class TodoCommandHandler :
        IRequestHandler<AddTodoCommand, TodoViewModel>,
        IRequestHandler<ToggleCompletedCommand, TodoViewModel>,
        IRequestHandler<UpdateTodoDescriptionCommand, TodoViewModel>,
        IRequestHandler<DeleteTodoCommand>
    {
        private readonly ITodoRepository repository;

        public TodoCommandHandler(ITodoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TodoViewModel> Handle(AddTodoCommand request, CancellationToken cancellationToken)
        {
            var entity = new Todo(request.Title, request.Description, request.Completed);
            entity = await repository.Add(entity);
            return new TodoViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Completed = entity.Completed,
                CompletedDate = entity.Completed ? entity.CompletedDate : default
            };
        }

        public async Task<TodoViewModel> Handle(ToggleCompletedCommand request, CancellationToken cancellationToken)
        {
            var todo = await repository.Get(request.Id);
            if (todo != null)
            {
                todo = todo.ToggleCompleted();
                todo = await repository.Update(todo);
                return new TodoViewModel
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Completed = todo.Completed,
                    CompletedDate = todo.Completed ? todo.CompletedDate : (DateTime?)null
                };
            }
            throw new TodoAppException("Todo not found.");
        }

        public async Task<TodoViewModel> Handle(UpdateTodoDescriptionCommand request, CancellationToken cancellationToken)
        {
            var todo = await repository.Get(request.Id);
            if (todo != null)
            {
                todo = todo.Update(request.Description);
                await repository.Update(todo);
                return new TodoViewModel
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Completed = todo.Completed,
                    CompletedDate = todo.Completed ? todo.CompletedDate : (DateTime?)null
                };
            }
            throw new TodoAppException("Todo not found.");
        }

        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await repository.Get(request.Id);
            if (todo == null) throw new TodoAppException("Todo not found.");
            await repository.Remove(request.Id);
            return Unit.Value;
        }
    }
}
