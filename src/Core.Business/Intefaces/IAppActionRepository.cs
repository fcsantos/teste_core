using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IAppActionRepository : IRepository<AppAction>
    {
        Task<IEnumerable<AppAction>> GetActionsByController(Guid Id);
    }
}