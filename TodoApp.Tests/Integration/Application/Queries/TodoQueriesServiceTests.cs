using System.Threading;
using System;
using System.Threading.Tasks;
using TodoApp.Application.Commands;
using TodoApp.Application.Queries;
using TodoApp.Infra.Repositories;
using TodoApp.Tests.Common;
using Xunit;
using TodoApp.Application.CommandHandlers;
using TodoApp.Infra.Context;
using System.Linq;

namespace TodoApp.Tests.Integration.Application.Queries
{
    public class TodoQueriesServiceTests : IDisposable
    {
        private readonly ITodoQueriesService service;
        private readonly TodoContext context;
        private readonly TodoCommandHandler handler;

        public TodoQueriesServiceTests()
        {
            context = ContextFactory.New();
            handler = new TodoCommandHandler(new TodoRepository(context));
            service = new TodoQueriesService(new TodoRepository(context));
        }

        //list all
        [Fact]
        public async Task ListAll()
        {
            for (int i = 0; i < 10; i++)
            {
                await handler.Handle(new AddTodoCommand($"Title{i}", $"Description{i}", false), CancellationToken.None);
            }
            var page = await service.ListAll(1, 10);
            Assert.NotNull(page);
            Assert.NotEmpty(page.Items);
            Assert.Equal(10, page.Total);
            Assert.Equal(10, page.Items.Count());
        }
        //list uncompleted
        [Fact]
        public async Task ListUncompleted()
        {
            for (int i = 0; i < 10; i++)
            {
                await handler.Handle(new AddTodoCommand($"Title{i}", $"Description{i}", i < 5), CancellationToken.None);
            }
            var page = await service.ListUncomplete(1, 10);
            Assert.NotNull(page);
            Assert.NotEmpty(page.Items);
            Assert.Equal(5, page.Total);
            Assert.Equal(5, page.Items.Count());
        }
        //search
        [Fact]
        public async Task SearchByDescription()
        {
            for (int i = 0; i < 10; i++)
            {
                await handler.Handle(new AddTodoCommand($"Title{i}", $"Description{i}", false), CancellationToken.None);
            }
            var page = await service.Search("description1", 1, 10);
            Assert.NotNull(page);
            Assert.NotEmpty(page.Items);
            Assert.Equal(1, page.Total);
            Assert.Single(page.Items);
            Assert.Equal("Description1", page.Items[0].Description);

            page = await service.Search("description5", 1, 10);
            Assert.NotNull(page);
            Assert.NotEmpty(page.Items);
            Assert.Equal(1, page.Total);
            Assert.Single(page.Items);
            Assert.Equal("Description5", page.Items[0].Description);
        }

        [Fact]
        public async Task SearchByTitle()
        {
            for (int i = 0; i < 10; i++)
            {
                await handler.Handle(new AddTodoCommand($"Title{i}", $"Description{i}", false), CancellationToken.None);
            }
            var page = await service.Search("title1", 1, 10);
            Assert.NotNull(page);
            Assert.NotEmpty(page.Items);
            Assert.Equal(1, page.Total);
            Assert.Single(page.Items);
            Assert.Equal("Title1", page.Items[0].Title);

            page = await service.Search("title5", 1, 10);
            Assert.NotNull(page);
            Assert.NotEmpty(page.Items);
            Assert.Equal(1, page.Total);
            Assert.Single(page.Items);
            Assert.Equal("Title5", page.Items[0].Title);
        }
        //get by id
        [Fact]
        public async Task GetById()
        {
            var todo = await handler.Handle(new AddTodoCommand($"Title", $"Description", false), CancellationToken.None);
            var item = await service.GetById(todo.Id);
            Assert.NotNull(item);
            Assert.Equal(todo.Id, item.Id);
            Assert.Equal(todo.Title, item.Title);
            Assert.Equal(todo.Description, item.Description);
        }

        public void Dispose()
        {
            context.Todos.RemoveRange(context.Todos.ToList());
            context.SaveChanges();
        }
    }
}