using System;
using TodoApp.Application.Queries;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Application.Queries
{
    public class GetTodoByIdQueryTests
    {
        [Fact]
        public void CreateGetTodoByIdQuerySuccess()
        {
            try
            {
                new GetTodoByIdQuery(Guid.NewGuid());
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void CreateGetTodoByIdQueryEmptyGuid()
        {
            Assert.Throws<DomainException>(() => new GetTodoByIdQuery(Guid.Empty));
        }
    }
}
