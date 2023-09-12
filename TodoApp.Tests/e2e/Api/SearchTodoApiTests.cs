using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;
using TodoApp.Infra.Context;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.e2e.Api
{
    public class SearchTodoAPITests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";

        public SearchTodoAPITests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
            context?.SaveChanges();
        }

        [Fact]
        public async Task SearchByOneTodo()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title",
                Description = Guid.NewGuid().ToString()
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();

            var data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/search?&criteria=Title&page=1&limit=10");
            Assert.NotNull(data.Items);
            Assert.Single(data.Items);
            Assert.Equal(payload.Title, data.Items[0].Title);
            Assert.Equal(payload.Description, data.Items[0].Description);
        }

        [Fact]
        public async Task SearchTenTodos()
        {
            const int totalItems = 10;
            for (int i = 0; i < totalItems; i++)
            {
                var payload = new AddTodoPayload
                {
                    Title = $"Title_{i}",
                    Description = Guid.NewGuid().ToString()
                };
                var response = await testContext.Post(URL, payload);
                response.EnsureSuccessStatusCode();
            }
            var data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/search?criteria=Title_&page=1&limit=10");
            Assert.NotNull(data.Items);
            Assert.Equal(totalItems, data.Total);
            Assert.Equal(10, data.Items.Count);
        }

        [Fact]
        public async Task SearchTodosWithPagination()
        {
            const int totalItems = 25;
            for (int i = 0; i < totalItems; i++)
            {
                var payload = new AddTodoPayload
                {
                    Title = $"Title_{i}",
                    Description = Guid.NewGuid().ToString()
                };
                var response = await testContext.Post(URL, payload);
                response.EnsureSuccessStatusCode();
            }
            var data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/search?criteria=Title_&page=1&limit=10");
            Assert.NotNull(data.Items);
            Assert.Equal(totalItems, data.Total);
            Assert.Equal(10, data.Items.Count);

            data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/search?criteria=Title_&page=2&limit=10");
            Assert.NotNull(data.Items);
            Assert.Equal(totalItems, data.Total);
            Assert.Equal(10, data.Items.Count);

            data = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}/search?criteria=Title_&page=3&limit=10");
            Assert.NotNull(data.Items);
            Assert.Equal(totalItems, data.Total);
            Assert.Equal(5, data.Items.Count);
        }
    }
}
