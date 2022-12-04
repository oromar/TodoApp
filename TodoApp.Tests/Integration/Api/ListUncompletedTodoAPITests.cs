using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;
using TodoApp.Infra.Context;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.Integration.Api
{
    public class ListUncompletedTodoAPITests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";

        public ListUncompletedTodoAPITests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
            context?.SaveChanges();
        }

        [Fact]
        public async Task ListOneTodo()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title",
                Description = Guid.NewGuid().ToString()
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();

            var data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/uncomplete?page=1&limit=10");
            Assert.NotNull(data.Items);
            Assert.Single(data.Items);
            Assert.Equal(payload.Title, data.Items[0].Title);
            Assert.Equal(payload.Description, data.Items[0].Description);
        }

        [Fact]
        public async Task ListTenTodos()
        {
            const int totalItems = 10;
            const int uncompletedItems = 6;
            for (int i = 0; i < totalItems; i++)
            {
                var payload = new AddTodoPayload
                {
                    Title = $"Title_{i}",
                    Description = Guid.NewGuid().ToString(),
                    Completed = i > 5
                };
                var response = await testContext.Post(URL, payload);
                response.EnsureSuccessStatusCode();
            }
            var data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/uncomplete?page=1&limit=10");
            Assert.NotNull(data.Items);
            Assert.Equal(uncompletedItems, data.Total);
            Assert.Equal(uncompletedItems, data.Items.Count);
        }

        [Fact]
        public async Task ListTodosWithPagination()
        {
            const int totalItems = 25;
            const int uncompletedItems = 16;
            for (int i = 0; i < totalItems; i++)
            {
                var payload = new AddTodoPayload
                {
                    Title = $"Title_{i}",
                    Description = Guid.NewGuid().ToString(),
                    Completed = i > 15
                };
                var response = await testContext.Post(URL, payload);
                response.EnsureSuccessStatusCode();
            }
            var data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/uncomplete?page=1&limit=5");
            Assert.NotNull(data.Items);
            Assert.Equal(uncompletedItems, data.Total);
            Assert.Equal(5, data.Items.Count);

            data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/uncomplete?page=2&limit=5");
            Assert.NotNull(data.Items);
            Assert.Equal(uncompletedItems, data.Total);
            Assert.Equal(5, data.Items.Count);

            data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/uncomplete?page=3&limit=5");
            Assert.NotNull(data.Items);
            Assert.Equal(uncompletedItems, data.Total);
            Assert.Equal(5, data.Items.Count);

            data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/uncomplete?page=4&limit=5");
            Assert.NotNull(data.Items);
            Assert.Equal(uncompletedItems, data.Total);
            Assert.Single(data.Items);
        }
    }
}
