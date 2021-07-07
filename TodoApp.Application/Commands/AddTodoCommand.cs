using MediatR;
using TodoApp.Domain;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands
{
    public class AddTodoCommand : IRequest<TodoViewModel>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool Completed { get; private set; }

        public AddTodoCommand(string title, string description, bool completed = false)
        {
            Todo.ValidateTitle(title);
            Todo.ValidateDescription(description);
            Title = title;
            Description = description;
            Completed = completed;
        }
    }
}
