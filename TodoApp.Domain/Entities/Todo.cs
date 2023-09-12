using System;
using TodoApp.Domain.Common;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Domain.Entities
{
    public class Todo : BaseEntity, IAggregateRoot
    {
        public const int MAX_LENGTH_TITLE = 30;
        public const int MAX_LENGTH_DESCRIPTION = 100;

        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool Completed { get; private set; }
        public DateTime CompletedDate { get; private set; }

        //EF
        public Todo()
        {

        }

        public Todo(string title, string description, bool completed = false)
        {
            ValidateTitle(title);
            ValidateDescription(description);
            Title = title;
            Description = description;
            Completed = completed;
            CompletedDate = completed ? DateTime.Now : default;
        }

        public static void ValidateId(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                throw new DomainException("Id is required.");
            }
        }

        public static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new DomainException("Title is required.");
            }
            if (title.Length > MAX_LENGTH_TITLE)
            {
                throw new DomainException($"Title should have max {MAX_LENGTH_TITLE} characters.");
            }
        }

        public static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException("Description is required.");
            }
            if (description.Length > MAX_LENGTH_DESCRIPTION)
            {
                throw new DomainException($"Description should have max {MAX_LENGTH_DESCRIPTION} characters.");
            }
        }

        public Todo ToggleCompleted()
        {
            Completed = !Completed;
            CompletedDate = Completed ? DateTime.Now : default;
            return this;
        }

        public Todo Update(string description)
        {
            ValidateDescription(description);
            Description = description;
            return this;
        }
    }
}
