using MediatR;
using System;
using TodoApp.Domain;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands
{
    public class UpdateTodoDescriptionCommand : IRequest<TodoViewModel>
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public UpdateTodoDescriptionCommand(Guid id, string description)
        {
            Todo.ValidateId(id);
            Todo.ValidateDescription(description);
            Id = id;
            Description = description;
        }
    }
}
