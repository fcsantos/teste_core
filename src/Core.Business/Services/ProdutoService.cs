using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUser _user;
         
        public ProdutoService(IProdutoRepository produtoRepository,
                              INotifier notificador,
                              IUser user) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _user = user;
        }

        public async Task Adicionar(Produto produto)
        {
            if (!ExecuteValidation(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Create(AuditColumns<Produto>(produto, "Create", _user.GetUserId()));
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecuteValidation(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Update(AuditColumns<Produto>(produto, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _produtoRepository.Delete(id);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}