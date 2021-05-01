using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business.Services
{
    public class EmergencyChannelService : BaseService, IEmergencyChannelService
    {
        private readonly IEmergencyChannelRepository _emergencyChannelRepository;
        private readonly IUser _user;

        public EmergencyChannelService(IEmergencyChannelRepository emergencyChannelRepository,
                                INotifier notifier, IUser user) : base(notifier)
        {
            _emergencyChannelRepository = emergencyChannelRepository;
            _user = user;
        }

        public async Task Create(EmergencyChannel emergencyChannel)
        {
            if (!ExecuteValidation(new EmergencyChannelValidation(), emergencyChannel)) return;

            if (_emergencyChannelRepository.Search(h => h.Name == emergencyChannel.Name).Result.Any())
            {
                Notification("Já existe um serviço de ajuda com o nome informado.");
                return;
            }

            await _emergencyChannelRepository.Create(AuditColumns<EmergencyChannel>(emergencyChannel, "Create", _user.GetUserId()));
        }

        public async Task Update(EmergencyChannel emergencyChannel)
        {
            if (!ExecuteValidation(new EmergencyChannelValidation(), emergencyChannel)) return;

            await _emergencyChannelRepository.Update(AuditColumns<EmergencyChannel>(emergencyChannel, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            var emergencyChannel = await _emergencyChannelRepository.GetById(id);

            if (emergencyChannel != null)
            {
                await _emergencyChannelRepository.Delete(id);
            }
        }

        public async Task Delete(EmergencyChannel emergencyChannel)
        {
            emergencyChannel.IsActive = emergencyChannel.IsActive ? false : true;
            await _emergencyChannelRepository.Update(AuditColumns<EmergencyChannel>(emergencyChannel, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _emergencyChannelRepository?.Dispose();
        }
    }
}
