using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TodoApp.Api.Payload;
using TodoApp.Infra.Context;
using TodoApp.Tests.Common;
using Xunit;

namespace TodoApp.Tests.Integration.Api
{
    public class CreateTodoAPITests
    {
        private readonly TestContext testContext = TestContext.Instance;
        private const string URL = "/api/todos";

        public CreateTodoAPITests()
        {
            var context = testContext.ServiceProvider.GetService(typeof(TodoContext)) as TodoContext;
            context?.Todos.RemoveRange(context?.Todos.ToList());
            context?.SaveChanges();
        }

        [Fact]
        public async Task CreateTodoSucess()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title1",
                Description = Guid.NewGuid().ToString()
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateTodoWithoutTitle()
        {
            var payload = new AddTodoPayload
            {
                Title = null,
                Description = Guid.NewGuid().ToString()
            };
            var response = await testContext.Post(URL, payload);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateTodoWithoutDescription()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title2",
                Description = null
            };
            var response = await testContext.Post(URL, payload);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateTodoCompletedFalse()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title3",
                Description = "Description",
                Completed = false,
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateTodoCompletedTrue()
        {
            var payload = new AddTodoPayload
            {
                Title = "Title4",
                Description = "Description",
                Completed = true,
            };
            var response = await testContext.Post(URL, payload);
            response.EnsureSuccessStatusCode();
        }
    }
}
