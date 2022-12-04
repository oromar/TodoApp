using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.CommandHandlers;
using TodoApp.Application.Common;
using TodoApp.Application.Queries;
using TodoApp.Domain.Entities;
using TodoApp.Infra.Context;
using TodoApp.Infra.Repositories;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.Application.QueyHandlers
{
    public class TodoQueryHandlerTests
    {
        private readonly TodoContext context;
        private readonly TodosQueryHandler handler;

        public TodoQueryHandlerTests()
        {
            context = ContextFactory.New();
            var repository = new TodoRepository(context);
            handler = new TodosQueryHandler(repository);
        }

        [Fact]
        public async Task GetTodosSuccess()
        {
            var todo1 = new Todo("Title1", "Description1");
            var todo2 = new Todo("Title2", "Description2");
            var todo3 = new Todo("Title3", "Description3");

            context.Todos.AddRange(todo1, todo2, todo3);
            context.SaveChanges();

            var todos = await handler.Handle(new GetListTodosQuery(1, 10), CancellationToken.None);
            Assert.Equal(3, todos.Total);
            var todoInDB1 = todos.Items.FirstOrDefault(a => a.Title == todo1.Title);
            Assert.NotNull(todoInDB1);
            Assert.Equal(todo1.Title, todoInDB1.Title);
            Assert.Equal(todo1.Description, todoInDB1.Description);
        }

        [Fact]
        public async Task GetTodosEmptyResult()
        {
            var todos = await handler.Handle(new GetListTodosQuery(1, 10), CancellationToken.None);
            Assert.Empty(todos.Items);
        }

        [Fact]
        public async Task GetTodoByIdSuccess()
        {
            var todo1 = new Todo("Title1", "Description1");
            context.Todos.Add(todo1);
            context.SaveChanges();
            var todos = await handler.Handle(new GetListTodosQuery(1, 10), CancellationToken.None);
            Assert.Single(todos.Items);
            var todoInDB1 = todos.Items.FirstOrDefault(a => a.Title == todo1.Title);
            var todo = await handler.Handle(new GetTodoByIdQuery(todoInDB1.Id), CancellationToken.None);
            Assert.NotNull(todo);
            Assert.Equal(todoInDB1.Id, todo.Id);
            Assert.Equal(todoInDB1.Title, todo.Title);
            Assert.Equal(todoInDB1.Description, todo.Description);
        }

        [Fact]
        public async Task GetTodoByIdEmptyResult()
        {
            await Assert.ThrowsAsync<TodoAppException>(() => handler.Handle(new GetTodoByIdQuery(Guid.NewGuid()), CancellationToken.None));
        }

        [Fact]
        public async Task GetTodosUncompletedSuccess()
        {
            var todo1 = new Todo("Title1", "Description1");
            var todo2 = new Todo("Title2", "Description2");
            todo2.ToggleCompleted();
            var todo3 = new Todo("Title3", "Description3");

            context.Todos.AddRange(todo1, todo2, todo3);
            context.SaveChanges();

            var todos = await handler.Handle(new GetUncompletedTodosQuery(1, 10), CancellationToken.None);
            Assert.Equal(2, todos.Total);
            var todoInDB1 = todos.Items.FirstOrDefault(a => a.Title == todo1.Title);
            Assert.NotNull(todoInDB1);
            Assert.Equal(todo1.Title, todoInDB1.Title);
            Assert.Equal(todo1.Description, todoInDB1.Description);
            Assert.DoesNotContain(todos.Items, a => a.Completed);
        }

        [Fact]
        public async Task GetTodosUncompletedEmptyResult()
        {
            var todo1 = new Todo("Title1", "Description1");
            todo1.ToggleCompleted();
            var todo2 = new Todo("Title2", "Description2");
            todo2.ToggleCompleted();
            var todo3 = new Todo("Title3", "Description3");
            todo3.ToggleCompleted();

            context.Todos.AddRange(todo1, todo2, todo3);
            context.SaveChanges();

            var todos = await handler.Handle(new GetUncompletedTodosQuery(1, 10), CancellationToken.None);
            Assert.Empty(todos.Items);
        }
    }
}
