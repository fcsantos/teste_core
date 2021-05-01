using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IAddressRepository _enderecoRepository;
        private readonly IUser _user;

        public FornecedorService(IFornecedorRepository fornecedorRepository, 
                                 IAddressRepository enderecoRepository,
                                 INotifier notificador,
                                 IUser user) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            _user = user;
        }

        public async Task Create(Fornecedor fornecedor)
        {
            if (!ExecuteValidation(new FornecedorValidation(), fornecedor)) return;

            if (_fornecedorRepository.Search(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notification("Já existe um fornecedor com este documento infomado.");
                return;
            }
            
            await _fornecedorRepository.Create(AuditColumns<Fornecedor, Address>(fornecedor, "Create",_user.GetUserId(), fornecedor.Address));
        }

        public async Task Update(Fornecedor fornecedor)
        {
            if (!ExecuteValidation(new FornecedorValidation(), fornecedor)) return;

            if (_fornecedorRepository.Search(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notification("Já existe um fornecedor com este documento infomado.");
                return;
            }

            await _fornecedorRepository.Update(AuditColumns<Fornecedor, Address>(fornecedor, "Update", _user.GetUserId(), fornecedor.Address));
        }

        public async Task UpdateEndereco(Address endereco)
        {
            if (!ExecuteValidation(new AddressValidation(), endereco)) return;

            await _enderecoRepository.Update(endereco);
        }

        public async Task Delete(Guid id)
        {
            if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any()) 
            {
                Notification("O fornecedor possui produtos cadastrados!");
                return;
            }

            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);

            await _fornecedorRepository.Delete(id);

            if (endereco != null)
            {
                await _enderecoRepository.Delete(endereco.Id);
            }
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}