using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Core.Api.ViewModels
{
    public class ServiceViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public Guid DoctorId { get; set; }
        [ScaffoldColumn(false)]
        public string DoctorName { get; set; }
        public Guid ServiceId { get; set; }
        public bool? IsActive { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }
        public ICollection<Guid> ServiceDoctors { get; set; }
    }
}
