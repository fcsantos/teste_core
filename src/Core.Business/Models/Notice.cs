using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Notice : Entity
    {
        
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
        public bool SendToAllUsers { get; set; }  
        public StatusNotice Status { get; set; }

        /* EF Relations */
        public ICollection<NoticeUser> NoticeUsers { get; set; }


    }
}