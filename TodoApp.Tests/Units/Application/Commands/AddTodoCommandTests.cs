using TodoApp.Application.Commands;
using TodoApp.Domain.Common;
using Xunit;

namespace TodoApp.Tests.Application.Commands
{
    public class AddTodoCommandTests
    {
        [Fact]
        public void CreateAddTodoCommandSuccess()
        {
            try
            {
                new AddTodoCommand("Title", "Description");
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
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
