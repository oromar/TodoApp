using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Domain;
using TodoApp.Infra.Context;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.Integration.Api
{
    public class DeleteTodoAPITests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";
        public DeleteTodoAPITests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
        }

        [Fact]
        public async Task DeleteTodoSuccess()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title",
                Description = Guid.NewGuid().ToString()
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();
            var todo = await response.Content.ReadAsAsync<TodoViewModel>();
            response = await testContext.Delete($"{URL}/{todo.Id}");
            response.EnsureSuccessStatusCode();

            response = await testContext.Get($"{URL}/{todo.Id}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTodoEmptyGuid()
        {
            var response = await testContext.Delete($"{URL}/{Guid.Empty}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTodoInexistentGuid()
        {
            var response = await testContext.Delete($"{URL}/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
