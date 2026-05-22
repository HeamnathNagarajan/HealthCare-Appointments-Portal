using System;
using System.Collections.Generic;
using System.Text;

namespace HealthcareApp.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; private set; }

        protected BaseEntity()
        {
            PublicId = Guid.NewGuid();
        }
    }
}