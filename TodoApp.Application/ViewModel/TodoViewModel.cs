using System;

namespace TodoApp.Domain
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
