using System;

namespace Domain.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; }

        protected Entity()
        {
            DateAdded = DateTime.UtcNow;
            IsDeleted = false;
            DateModified = DateTime.UtcNow;
        }
    }
}
