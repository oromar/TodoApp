using System.Linq;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;
using TodoApp.Infra.Context;
using TodoApp.Infra.Repositories;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.Infra
{
    public class TodoRepositoryTests
    {
        private readonly TodoContext context;
        private readonly TodoRepository repository;

        public TodoRepositoryTests()
        {
            context = ContextFactory.New();
            repository = new TodoRepository(context);
        }

        [Fact]
        public void CreateRepositorySuccess()
        {
            try
            {
                new TodoRepository(context);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
                throw;
            }
        }

        [Fact]
        public async Task TestAdd()
        {
            var todo = new Todo("Title", "Description");
            await repository.Add(todo);
            Assert.True(context.Todos.Any(a => a.Id == todo.Id));
        }

        [Fact]
        public async Task TestUpdate()
        {
            var todo = new Todo("Title", "Description");
            await repository.Add(todo);
            Assert.True(context.Todos.Any(a => a.Id == todo.Id));
            todo.Update("new description");
            await repository.Update(todo);
            Assert.True(context.Todos.Any(a => a.Id == todo.Id && a.Description == "new description"));
        }

        [Fact]
        public async Task TestList()
        {
            var todo1 = new Todo("Title1", "Description1");
            await repository.Add(todo1);
            Assert.True(context.Todos.Any(a => a.Id == todo1.Id));
            var todo2 = new Todo("Title2", "Description2");
            await repository.Add(todo2);
            Assert.True(context.Todos.Any(a => a.Id == todo2.Id));
            var todo3 = new Todo("Title3", "Description3");
            await repository.Add(todo3);
            Assert.True(context.Todos.Any(a => a.Id == todo3.Id));
            var todos = await repository.List();
            Assert.Equal(3, todos.Count);
        }

        [Fact]
        public async Task TestListWithPredicate()
        {
            var todo1 = new Todo("Title1", "Description1");
            await repository.Add(todo1);
            Assert.True(context.Todos.Any(a => a.Id == todo1.Id));
            var todo2 = new Todo("Title2", "Description2");
            await repository.Add(todo2);
            Assert.True(context.Todos.Any(a => a.Id == todo2.Id));
            var todo3 = new Todo("Title3", "Description3");
            await repository.Add(todo3);
            Assert.True(context.Todos.Any(a => a.Id == todo3.Id));
            var todos = await repository.List(a => a.Title == "Title1");
            Assert.Single(todos);
            Assert.Equal("Title1", todos[0].Title);
            Assert.Equal("Description1", todos[0].Description);
        }

        [Fact]
        public async Task TestGetById()
        {
            var todo1 = new Todo("Title1", "Description1");
            await repository.Add(todo1);
            Assert.True(context.Todos.Any(a => a.Id == todo1.Id));
            var todo2 = new Todo("Title2", "Description2");
            await repository.Add(todo2);
            Assert.True(context.Todos.Any(a => a.Id == todo2.Id));
            var todo3 = new Todo("Title3", "Description3");
            await repository.Add(todo3);
            Assert.True(context.Todos.Any(a => a.Id == todo3.Id));
            var todo = await repository.Get(todo1.Id);
            Assert.Equal(todo1.Id, todo.Id);
            Assert.Equal(todo1.Title, todo.Title);
            Assert.Equal(todo1.Description, todo.Description);
        }

        [Fact]
        public async Task TestRemove()
        {
            var todo1 = new Todo("Title1", "Description1");
            await repository.Add(todo1);
            Assert.True(context.Todos.Any(a => a.Id == todo1.Id));
            await repository.Remove(todo1.Id);
            Assert.Empty(context.Todos);
        }
    }
}
