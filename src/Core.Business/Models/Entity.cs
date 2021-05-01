using System;

namespace Core.Business.Models
{
    public abstract class Entity
    {
        protected Entity()
        { 
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}