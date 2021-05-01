using Core.Business.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> ObterEnderecoPorFornecedor(Guid fornecedorId); 
    }
}