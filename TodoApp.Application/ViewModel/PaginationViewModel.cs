using System.Collections.Generic;

namespace TodoApp.Application.ViewModel
{
    public class PaginationViewModel<T>
    {
        public int Total { get; private set; }
        public int Page { get; private set; }
        public int Limit { get; private set; }
        public List<T> Items { get; private set; }

        public PaginationViewModel(List<T> items, int total, int page, int limit)
        {
            Total = total;
            Page = page;
            Limit = limit;
            Items = items;
        }
    }
}
