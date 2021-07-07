
using MediatR;
using System;
using TodoApp.Domain;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Queries
{
    public class GetTodoByIdQuery : IRequest<TodoViewModel>
    {
        public Guid Id { get; private set; }
        public GetTodoByIdQuery(Guid id)
        {
            Todo.ValidateId(id);
            Id = id;
        }
    }
}
