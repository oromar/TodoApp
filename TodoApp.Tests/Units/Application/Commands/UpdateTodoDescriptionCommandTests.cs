using System;
using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Units.Application.Commands
{
    public class UpdateTodoDescriptionCommandTests
    {
        [Fact]
        public void CreateUpdateTodoDescriptionCommandSuccess()
        {
            var command = new UpdateTodoDescriptionCommand(Guid.NewGuid(), "description");
            Assert.NotNull(command);
        }

        [Fact]
        public void CreateUpdateTodoDescriptionEmptyGuid()
        {
            Assert.Throws<DomainException>(() => new UpdateTodoDescriptionCommand(Guid.Empty, "description"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void CreateUpdateTodoDescriptionInvalidDescription(string value)
        {
            Assert.Throws<DomainException>(() => new UpdateTodoDescriptionCommand(Guid.NewGuid(), value));
        }
    }
}

