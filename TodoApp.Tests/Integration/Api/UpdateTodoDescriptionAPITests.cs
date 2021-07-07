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

namespace TodoApp.Tests.Integration.Api
{
    public class UpdateTodoDescriptionAPITests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";

        public UpdateTodoDescriptionAPITests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
            context?.SaveChanges();
        }

        [Fact]
        public async Task UpdateDescriptionSuccess()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title",
                Description = "Description",
            };

            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();

            var todoInDB = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}?page=1&limit=10");
            Assert.NotNull(todoInDB);
            Assert.Equal(1, todoInDB.Total);
            Assert.Single(todoInDB.Items);

            var updatePayload = new UpdateTodoDescriptionPayload
            {
                Description = "new description"
            };
            var updateResponse = await testContext.Put($"{URL}/{todoInDB.Items[0].Id}", updatePayload);
            updateResponse.EnsureSuccessStatusCode();

            var todo = await testContext.Get<TodoViewModel>($"{URL}/{todoInDB.Items[0].Id}");
            Assert.NotNull(todo);
            Assert.Equal("new description", todo.Description);
        }

        [Fact]
        public async Task UpdateDescriptionEmptyNewDescription()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title",
                Description = "Description",
            };

            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();

            var todoInDB = await testContext.Get<PaginationViewModel<TodoViewModel>>($"{URL}?page=1&limit=10");
            Assert.NotNull(todoInDB);
            Assert.Equal(1, todoInDB.Total);
            Assert.Single(todoInDB.Items);

            var updatePayload = new UpdateTodoDescriptionPayload
            {
                Description = ""
            };
            var updateResponse = await testContext.Put($"{URL}/{todoInDB.Items[0].Id}", updatePayload);
            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
        }

        [Fact]
        public async Task UpdateDescriptionEmptyGuid()
        {
            var updateResponse = await testContext.Put($"{URL}/{Guid.Empty}", new UpdateTodoDescriptionPayload { Description = "new description" });
            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
        }

        [Fact]
        public async Task UpdateDescriptionInexistentGuid()
        {
            var updateResponse = await testContext.Put($"{URL}/{Guid.NewGuid()}", new UpdateTodoDescriptionPayload { Description = "new description" });
            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
        }
    }
}
