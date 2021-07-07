using System;
using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Application.Commands
{
    public class UpdateTodoDescriptionCommandTests
    {
        [Fact]
        public void CreateUpdateTodoDescriptionCommandSuccess()
        {
            try
            {
                new UpdateTodoDescriptionCommand(Guid.NewGuid(), "description");
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
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

