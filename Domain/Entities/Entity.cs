using System;

namespace Domain.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            DateAdded = DateTime.UtcNow;
            IsDeleted = false;
            DateModified = DateTime.UtcNow;
        }
    }
}
