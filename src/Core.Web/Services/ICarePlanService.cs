using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface ICarePlanService
    {
        Task<IEnumerable<CarePlanViewModel>> GetAll(Guid id);

        Task<IEnumerable<CarePlanViewModel>> GetAllExceptDoctor(Guid id);

        Task<CarePlanViewModel> GetById(Guid id);

        Task<IEnumerable<CarePlanViewModel>> GetAllCarePlansByPacientId();

        Task<ResponseResult> Create(CarePlanViewModel carePlanViewModel);

        Task<ResponseResult> Update(CarePlanViewModel carePlanViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
