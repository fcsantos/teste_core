using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MyDbContext context) : base(context) { }

        public async Task<Address> ObterEnderecoPorFornecedor(Guid fornecedorId) 
        {
            return await Db.Adresses.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(a => a.Fornecedor.Id == fornecedorId);
        }
    }
}