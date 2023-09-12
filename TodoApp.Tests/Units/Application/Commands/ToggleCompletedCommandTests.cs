using System;
using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Units.Application.Commands
{
    public class ToggleCompletedCommandTests
    {
        [Fact]
        public void CreateToggleCompletedCommandSuccess()
        {
            var command = new ToggleCompletedCommand(Guid.NewGuid());
            Assert.NotNull(command);
        }

        [Fact]
        public void CreateToggleCompletedCommandEmptyGuid()
        {
            Assert.Throws<DomainException>(() => new ToggleCompletedCommand(Guid.Empty));
        }
    }
}
