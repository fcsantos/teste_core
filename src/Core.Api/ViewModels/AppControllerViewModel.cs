using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.ViewModels
{
    public class AppControllerViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome do Controller")]
        public string ControllerName { get; set; }

        public IEnumerable<AppActionViewModel> Actions { get; set; }
    }
}
