using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Units.Application.Commands
{
    public class AddTodoCommandTests
    {
        [Fact]
        public void CreateAddTodoCommandSuccess()
        {
            var command = new AddTodoCommand("Title", "Description");
            Assert.NotNull(command);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void TestCreateAddTodoCommandInvalidDescription(string value)
        {
            Assert.Throws<DomainException>(() => new AddTodoCommand("title", value));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void TestCreateAddTodoCommandInvalidTitle(string value)
        {
            Assert.Throws<DomainException>(() => new AddTodoCommand(value, "description"));
        }
    }
}
