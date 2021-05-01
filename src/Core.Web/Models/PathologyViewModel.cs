using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class PathologyViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [DisplayName("Patologia")]
        public Guid? ParentPathologyId { get; set; }

        [ScaffoldColumn(false)]
        public string ParentName { get; set; }

        public IEnumerable<PathologyViewModel> ParentPathologies { get; set; }

        public IEnumerable<PathologyViewModel> SubPathologies { get; set; }
    }
}
