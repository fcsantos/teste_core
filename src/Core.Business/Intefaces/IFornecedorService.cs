using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{ 
    public interface IFornecedorService : IDisposable
    {
        Task Create(Fornecedor fornecedor);
        Task Update(Fornecedor fornecedor);
        Task Delete(Guid id);

        Task UpdateEndereco(Address endereco);
    }
}