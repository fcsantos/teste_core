using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class AppActionService : BaseService, IAppActionService
    {
        private readonly IAppActionRepository _actionRepository;
        private readonly IUser _user;

        public AppActionService(IAppActionRepository actionRepository, 
                                 INotifier notificador,
                                 IUser user) : base(notificador)
        {
            _actionRepository = actionRepository;
            _user = user;
        }

        public async Task Create(AppAction action)
        {
            if (!ExecuteValidation(new ActionValidation(), action)) return;

            if (_actionRepository.Search(f => f.ActionName == action.ActionName).Result.Any())
            {
                Notification("Já existe uma action com este Nome.");
                return;
            }
            
            await _actionRepository.Create(AuditColumns<AppAction>(action, "Create",_user.GetUserId()));
        }

        public async Task Update(AppAction action)
        {
            if (!ExecuteValidation(new ActionValidation(), action)) return;

            if (_actionRepository.Search(s => s.ActionName == action.ActionName && s.Id != action.Id).Result.Any())
            {
                Notification("Já existe um action com este Nome.");
                return;
            }

            await _actionRepository.Update(AuditColumns<AppAction>(action, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _actionRepository.Delete(id);
        }

        public void Dispose()
        {
            _actionRepository?.Dispose();
        }
    }
}