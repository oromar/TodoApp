using MediatR;
using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands
{
    public class DeleteTodoCommand : IRequest
    {
        public Guid Id { get; private set; }
        public DeleteTodoCommand(Guid id)
        {
            Todo.ValidateId(id);
            Id = id;
        }
    }
}
