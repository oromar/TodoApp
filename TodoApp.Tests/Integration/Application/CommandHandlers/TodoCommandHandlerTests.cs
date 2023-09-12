using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.CommandHandlers;
using TodoApp.Application.Commands;
using TodoApp.Application.Common;
using TodoApp.Domain.Entities;
using TodoApp.Infra.Context;
using TodoApp.Infra.Repositories;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.Integration.Application.CommandHandlers
{
    public class TodoCommandHandlerTests
    {
        private readonly TodoContext context;
        private readonly TodoRepository repository;
        private readonly TodoCommandHandler handler;

        public TodoCommandHandlerTests()
        {
            context = ContextFactory.New();
            handler = new TodoCommandHandler(new TodoRepository(context));
        }

        [Fact]
        public void CreateHandlerSuccess()
        {
            try
            {
                new TodoCommandHandler(repository);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task CreateTodoSuccess()
        {
            var command = new AddTodoCommand("Title", "Description", false);
            var todo = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo);
            Assert.NotEqual(Guid.Empty, todo.Id);
            Assert.True(context.Todos.Any(b => b.Id == todo.Id));
        }

        [Fact]
        public async Task CreateCompletedTodoSuccess()
        {
            var command = new AddTodoCommand("Title", "Description", true);
            var todo = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo);
            Assert.NotEqual(Guid.Empty, todo.Id);
            Assert.True(todo.Completed);
            Assert.True(context.Todos.Any(b => b.Id == todo.Id && b.Completed));
        }

        [Fact]
        public async Task CreateCompletedSameTitleTwice()
        {
            var command = new AddTodoCommand("Title", "Description", true);
            var todo = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo);
            Assert.NotEqual(Guid.Empty, todo.Id);
            Assert.True(todo.Completed);
            Assert.True(context.Todos.Any(b => b.Id == todo.Id && b.Completed));
            await Assert.ThrowsAsync<TodoAppException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ToggleCompletedTodoSuccess()
        {
            var todo = new Todo("Title", "Description", false);
            context.Todos.Add(todo);
            context.SaveChanges();

            var command = new ToggleCompletedCommand(todo.Id);
            var todo1 = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo1);
            Assert.True(todo1.Completed);
            Assert.True(context.Todos.Any(b => b.Id == todo1.Id && b.Completed));

            command = new ToggleCompletedCommand(todo.Id);
            todo1 = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo1);
            Assert.False(todo1.Completed);
            Assert.True(context.Todos.Any(b => b.Id == todo1.Id && !b.Completed));
        }

        [Fact]
        public async Task ToggleCompletedTodoInexistentId()
        {
            var todo = new Todo("Title", "Description", false);
            context.Todos.Add(todo);
            context.SaveChanges();

            var command = new ToggleCompletedCommand(Guid.NewGuid());
            await Assert.ThrowsAsync<TodoAppException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateTodoDescriptionSuccess()
        {
            const string description = "Description";
            const string newDescription = "new Description";
            var todo = new Todo("Title", description, false);
            context.Todos.Add(todo);
            context.SaveChanges();

            var command = new UpdateTodoDescriptionCommand(todo.Id, newDescription);
            var todo1 = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo1);
            Assert.Equal(newDescription, todo1.Description);
            Assert.True(context.Todos.Any(b => b.Id == todo1.Id && b.Description == newDescription));
        }

        [Fact]
        public async Task UpdateDescriptionTodoInexistentId()
        {
            var todo = new Todo("Title", "Description", false);
            context.Todos.Add(todo);
            context.SaveChanges();

            var command = new UpdateTodoDescriptionCommand(Guid.NewGuid(), "new description");
            await Assert.ThrowsAsync<TodoAppException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteTodo()
        {
            var command = new AddTodoCommand("Title", "Description", false);
            var todo = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(todo);
            Assert.NotEqual(Guid.Empty, todo.Id);
            Assert.True(context.Todos.Any(b => b.Id == todo.Id));

            var command2 = new DeleteTodoCommand(todo.Id);
            await handler.Handle(command2, CancellationToken.None);
            Assert.True(!context.Todos.Any(b => b.Id == todo.Id));
        }

        [Fact]
        public async Task DeleteTodoInexistentId()
        {
            await Assert.ThrowsAsync<TodoAppException>(() => handler.Handle(new DeleteTodoCommand(Guid.NewGuid()), CancellationToken.None));
        }
    }
}
