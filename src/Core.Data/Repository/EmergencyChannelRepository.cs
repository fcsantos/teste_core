using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;

namespace Core.Data.Repository
{
    public class EmergencyChannelRepository : Repository<EmergencyChannel>, IEmergencyChannelRepository
    {
        public EmergencyChannelRepository(MyDbContext context) : base(context) { }
    }
}
