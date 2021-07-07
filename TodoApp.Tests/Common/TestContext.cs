using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApp.Api;
using TodoApp.Infra.Context;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TodoApp.Tests.Common
{
    public class TestContext : IDisposable
    {
        private static TestServer _server;
        private readonly HttpClient client;
        public IServiceProvider ServiceProvider;

        private TestContext()
        {
            var webBuilder = new WebHostBuilder().UseStartup<Startup>()
                                                 .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.None))
                                                 .UseEnvironment("Development")
                                                 .ConfigureServices(services =>
                                                     services.AddDbContext<TodoContext>(options =>
                                                        options.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Singleton)
                                                 );
            _server = new TestServer(webBuilder);
            ServiceProvider = _server.Host.Services;
            client = _server.CreateClient();
        }

        public static TestContext Instance { get; } = new TestContext();

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent value) => await client.PostAsync($"{requestUri}", value);
        public async Task<HttpResponseMessage> Post<T>(string requestUri, T value) => await client.PostAsJsonAsync($"{requestUri}", value);
        public async Task<HttpResponseMessage> Put<T>(string requestUri, T value) => await client.PutAsJsonAsync($"{requestUri}", value);
        public async Task<HttpResponseMessage> Delete(string requestUri) => await client.DeleteAsync($"{requestUri}");
        public async Task<HttpResponseMessage> Get(string requestUri) => await client.GetAsync($"{requestUri}");
        public async Task<T> Get<T>(string requestUri)
        {
            var response = await Get(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _server?.Dispose();
            client?.Dispose();
        }
    }
}
