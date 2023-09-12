using System;
using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Units.Application.Commands
{
    public class DeleteTodoCommandTests
    {
        [Fact]
        public void CreateDeleteTodoCommandSuccess()
        {
            var command = new DeleteTodoCommand(Guid.NewGuid());
            Assert.NotNull(command);
        }

        [Fact]
        public void CreateDeleteTodoCommandEmptyGuid()
        {
            Assert.Throws<DomainException>(() => new DeleteTodoCommand(Guid.Empty));
        }
    }
}
