using System;

namespace TodoApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        public override bool Equals(object obj)
        {
            return obj != null && (obj as BaseEntity).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 73;
        }
    }
}
