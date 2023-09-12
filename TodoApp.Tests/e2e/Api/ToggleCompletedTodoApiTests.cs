using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Application.ViewModel;
using TodoApp.Domain;
using TodoApp.Infra.Context;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.e2e.Api
{
    public class ToggleCompletedTodoApiTests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";
        public ToggleCompletedTodoApiTests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
            context?.SaveChanges();
        }


        [Fact]
        public async Task ToggleCompletedSuccess()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title1",
                Description = "Description",
            };

            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();

            var todoInDB = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}?page=1&limit=10");
            Assert.NotNull(todoInDB);
            Assert.Equal(1, todoInDB.Total);
            Assert.Single(todoInDB.Items);

            var toggleResponse = await testContext.Put($"{URL}/toggleCompleted/{todoInDB.Items[0].Id}", new { });
            toggleResponse.EnsureSuccessStatusCode();

            var todo = await testContext.Get<TodoViewModel>($"{URL}/{todoInDB.Items[0].Id}");
            Assert.NotNull(todo);
            Assert.True(todo.Completed);
        }

        [Fact]
        public async Task ToggleEmptyId()
        {
            var toggleResponse = await testContext.Put($"{URL}/toggleCompleted/{Guid.Empty}", new { });
            Assert.Equal(HttpStatusCode.BadRequest, toggleResponse.StatusCode);
        }

        [Fact]
        public async Task ToggleInexistentId()
        {
            var toggleResponse = await testContext.Put($"{URL}/toggleCompleted/{Guid.NewGuid()}", new { });
            Assert.Equal(HttpStatusCode.BadRequest, toggleResponse.StatusCode);
        }
    }
}
