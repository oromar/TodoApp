using MediatR;
using System;
using TodoApp.Domain;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands
{
    public class ToggleCompletedCommand : IRequest<TodoViewModel>
    {
        public Guid Id { get; private set; }
        public ToggleCompletedCommand(Guid id)
        {
            Todo.ValidateId(id);
            Id = id;
        }
    }
}
