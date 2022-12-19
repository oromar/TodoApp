using TodoApp.Application.Queries;
using TodoApp.Infra.Repositories;
using TodoApp.Tests.Common;

namespace TodoApp.Tests.Units.Application.Queries
{
    public class TodoQueriesServiceTests
    {
        private readonly ITodoQueriesService service;
        public TodoQueriesServiceTests()
        {
            var context = ContextFactory.New();
            var repository = new TodoRepository(context);
            service = new TodoQueriesService(repository);

        }
    }
}