using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Categoria : Entity
    {
        public Guid? CategoriaId { get; set; } 
        public string Nome { get; set; }


        /* EF Relation */
        public Categoria SubCategoria { get; set; }
        public IEnumerable<Categoria> SubCategorias { get; set; }
    }
}
