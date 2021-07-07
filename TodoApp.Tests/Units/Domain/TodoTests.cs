using System;
using TodoApp.Domain.Common;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Tests.Domain
{
    public class TodoTests
    {
        [Fact]
        public void CreateSuccess()
        {
            try
            {
                new Todo("Title", "Description");
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void CreateEmptySuccess()
        {
            try
            {
                new Todo();
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TryCreateTodoWithoutTitle()
        {
            Assert.Throws<DomainException>(() => new Todo(null, "Description"));
        }

        [Fact]
        public void TryCreateTodoWithoutDescription()
        {
            Assert.Throws<DomainException>(() => new Todo("Title", null));
        }

        [Fact]
        public void TryCreateTodoWithTitleMaxChars()
        {
            try
            {
                new Todo(new string('a', Todo.MAX_LENGTH_TITLE), "Description");
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TryCreateTodoWithDescriptionMaxChars()
        {
            try
            {
                new Todo("Title", new string('a', Todo.MAX_LENGTH_DESCRIPTION));
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TryCreateTodoWithTitleMoreThanMaxChars()
        {
            Assert.Throws<DomainException>(() => new Todo(new string('a', Todo.MAX_LENGTH_TITLE + 1), "Description"));
        }

        [Fact]
        public void TryCreateTodoWithDescriptionMoreThanMaxChars()
        {
            Assert.Throws<DomainException>(() => new Todo("Title", new string('a', Todo.MAX_LENGTH_DESCRIPTION + 1)));
        }

        [Fact]
        public void TestValidateIdSuccess()
        {
            try
            {
                Todo.ValidateId(Guid.NewGuid());
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void TestValidateIdEmpty()
        {
            Assert.Throws<DomainException>(() => Todo.ValidateId(Guid.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void TestValidateTitleInvalid(string value)
        {
            Assert.Throws<DomainException>(() => Todo.ValidateTitle(value));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void TestValidateDescriptionInvalid(string value)
        {
            Assert.Throws<DomainException>(() => Todo.ValidateDescription(value));
        }

        [Fact]
        public void TestValidateToggleCompleted()
        {
            var todo = new Todo("Title", "Description");
            Assert.False(todo.Completed);
            Assert.Equal(default, todo.CompletedDate);
            todo.ToggleCompleted();
            Assert.True(todo.Completed);
            Assert.NotEqual(default, todo.CompletedDate);
            todo.ToggleCompleted();
            Assert.False(todo.Completed);
            Assert.Equal(default, todo.CompletedDate);
        }

        [Fact]
        public void TestValidateUpdateDescription()
        {
            const string description = "Description";
            const string newDescription = "new Description";

            var todo = new Todo("Title", description);
            Assert.Equal(description, todo.Description);
            todo.Update(newDescription);
            Assert.Equal(newDescription, todo.Description);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void TestValidateUpdateDescriptionWithInvalidDescription(string newDescription)
        {
            var todo = new Todo("Title", "Description");
            Assert.Throws<DomainException>(() => todo.Update(newDescription));
        }

        [Fact]
        public void TestTodoEquals()
        {
            var todo1 = new Todo("Title", "Description");
            var todo2 = new Todo("Title", "Description");
            Assert.NotEqual(todo1, todo2);
            Assert.NotEqual(todo1.GetHashCode(), todo2.GetHashCode());
        }
    }
}
