using System;

namespace Core.Business.Models.DTO
{
    public class NoticeUserDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool SendToAllUsers { get; set; }
        public string Status { get; set; }
    }
}
