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
    public class GetTodoByIdAPITests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";
        public GetTodoByIdAPITests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
        }

        [Fact]
        public async Task GetTodoByIdSuccess()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title",
                Description = Guid.NewGuid().ToString()
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();
            var todo = await response.Content.ReadAsAsync<TodoViewModel>();
            var id = todo.Id;
            todo = await testContext.Get<TodoViewModel>($"{URL}/{id}");
            Assert.NotNull(todo);
            Assert.Equal(id, todo.Id);
        }

        [Fact]
        public async Task GetTodoByIdEmptyGuid()
        {
            var response = await testContext.Get($"{URL}/{Guid.Empty}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetTodoByIdInexistentGuid()
        {
            var response = await testContext.Get($"{URL}/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
