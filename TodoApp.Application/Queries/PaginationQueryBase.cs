using TodoApp.Application.Common;

namespace TodoApp.Application.Queries
{
    public class PaginationQueryBase
    {
        public const int MAX_LIMIT_OF_ROWS = 100;
        public int Page { get; }
        public int Limit { get; }
        public int Offset { get; }
        public PaginationQueryBase(int page, int limit)
        {
            Page = page;
            Limit = limit;
            Validate();
            Offset = (Page - 1) * Limit;
        }
        private void Validate()
        {
            if (Page <= 0) throw new TodoAppException("Page number should be greather than 0.");
            if (Limit <= 0) throw new TodoAppException("Limit of rows number should be greather than 0.");
            if (Limit > MAX_LIMIT_OF_ROWS) throw new TodoAppException("Limit of rows number should be equal or less than 100.");
        }
    }
}
