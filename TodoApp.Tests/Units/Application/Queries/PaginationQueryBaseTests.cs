using TodoApp.Application.Common;
using TodoApp.Application.Queries;
using Xunit;

namespace TodoApp.Tests.Application.Queries
{
    public class PaginationQueryBaseTests
    {
        [Fact]
        public void CreatePaginationQueryBaseSuccess()
        {
            try
            {
                new PaginationQueryBase(1, 10);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void CreatePaginationQueryBaseInvalidPage()
        {
            Assert.Throws<TodoAppException>(() => new PaginationQueryBase(-1, 10));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        [InlineData(1000)]
        public void CreatePaginationQueryBaseInvalidLimit(int limit)
        {
            Assert.Throws<TodoAppException>(() => new PaginationQueryBase(1, limit));
        }
    }
}
