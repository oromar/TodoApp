using System;
using TodoApp.Api.Validators;
using Xunit;

namespace TodoApp.Tests.Units.Api.Validators
{
    public class IdValidatorTests
    {
        [Fact]
        public void TryValidateValidId()
        {
            var validator = new IdValidator();
            var result = validator.IsValid(Guid.NewGuid());
            Assert.True(result);
        }

        [Fact]
        public void TryValidatNullId()
        {
            var validator = new IdValidator();
            var result = validator.IsValid(null);
            Assert.False(result);
        }

        [Fact]
        public void TryValidatEmptyId()
        {
            var validator = new IdValidator();
            var result = validator.IsValid(Guid.Empty);
            Assert.False(result);
        }

        [Fact]
        public void TryValidatNotGuid()
        {
            var validator = new IdValidator();
            var result = validator.IsValid("teste");
            Assert.False(result);
        }
    }
}
