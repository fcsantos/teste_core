using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Api.ViewModels
{
    public class EmergencyChannelViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Phone]
        [Display(Name = "Telemóvel")]
        public string Cell { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Ordenação")]
        public int sortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
