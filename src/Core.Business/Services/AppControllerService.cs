using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class AppControllerService : BaseService, IAppControllerService
    {
        private readonly IAppControllerRepository _controllerRepository;        
        private readonly IUser _user;
         
        public AppControllerService(IAppControllerRepository controllerRepository,                                
                                INotifier notificador,
                                IUser user) : base(notificador)
        {
            _controllerRepository = controllerRepository;
            _user = user;
        }

        public async Task Create(AppController controller)
        {
            if (!ExecuteValidation(new ControllerValidation(), controller)) return;

            if (_controllerRepository.Search(s => s.ControllerName == controller.ControllerName).Result.Any())
            {
                Notification("Já existe uma Controller com este Nome.");
                return;
            }

            await _controllerRepository.Create(AuditColumns<AppController>(controller, "Create", _user.GetUserId()));
        }

        public async Task Update(AppController controller)
        {
            if (!ExecuteValidation(new ControllerValidation(), controller)) return;

            if (_controllerRepository.Search(s => s.ControllerName == controller.ControllerName && s.Id != controller.Id).Result.Any())
            {
                Notification("Já existe uma Controller com este Nome.");
                return;
            }

            await _controllerRepository.Update(AuditColumns<AppController>(controller, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            if (_controllerRepository.GetControllerActions(id).Result.Actions.Any())
            {
                Notification("O Controller possui actions cadastrados!");
                return;
            }

            await _controllerRepository.Delete(id);

        }

        public void Dispose()
        {
            _controllerRepository?.Dispose();
        }
    }
}
