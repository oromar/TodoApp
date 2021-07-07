using System;
using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Application.Commands
{
    public class DeleteTodoCommandTests
    {
        [Fact]
        public void CreateDeleteTodoCommandSuccess()
        {
            try
            {
                new DeleteTodoCommand(Guid.NewGuid());
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void CreateDeleteTodoCommandEmptyGuid()
        {
            Assert.Throws<DomainException>(() => new DeleteTodoCommand(Guid.Empty));
        }
    }
}
