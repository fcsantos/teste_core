using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Models
{
    public class EmergencyChannel : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Cell { get; set; }

        public int sortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
