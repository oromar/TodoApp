using System;
using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Application.Commands
{
    public class ToggleCompletedCommandTests
    {
        [Fact]
        public void CreateToggleCompletedCommandSuccess()
        {
            try
            {
                new ToggleCompletedCommand(Guid.NewGuid());
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void CreateToggleCompletedCommandEmptyGuid()
        {
            Assert.Throws<DomainException>(() => new ToggleCompletedCommand(Guid.Empty));
        }
    }
}
