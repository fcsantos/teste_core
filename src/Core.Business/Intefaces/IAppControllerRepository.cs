using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{ 
    public interface IAppControllerRepository : IRepository<AppController>
    {

        Task<AppController> GetControllerActions(Guid id);

    }
}