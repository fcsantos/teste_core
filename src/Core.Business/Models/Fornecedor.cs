using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Fornecedor : Entity
    {
        public Guid AddressId { get; set; } 
        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public bool Ativo { get; set; }

        /* EF Relations */
        public IEnumerable<Produto> Produtos { get; set; }

        public Address Address { get; set; }
    }
}