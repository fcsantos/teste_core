using Core.Business.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IEmergencyChannelService : IDisposable
    {
        Task Create(EmergencyChannel emergencyChannel);
        Task Update(EmergencyChannel emergencyChannel);
        Task Delete(Guid id);
        Task Delete(EmergencyChannel emergencyChannel);
    }
}
