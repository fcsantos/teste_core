using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Core.Api.ViewModels
{
    public class InquiryViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        [ScaffoldColumn(false)]
        public string Ativo { get; set; }
        public ICollection<QuestionViewModel> Questions { get; set; }
    }
}