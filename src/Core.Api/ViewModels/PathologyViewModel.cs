using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Api.ViewModels
{
    public class PathologyViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }
        public Guid? ParentPathologyId { get; set; }

        [ScaffoldColumn(false)]
        public string ParentName { get; set; }

        public IEnumerable<PathologyViewModel> SubPathologies { get; set; }
    }
}
