using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IObservationService
    {
        Task<IEnumerable<ObservationViewModel>> GetAll(Guid id);

        Task<IEnumerable<ObservationViewModel>> GetAllExceptDoctor(Guid id);

        Task<ObservationViewModel> GetById(Guid id);

        Task<ResponseResult> Create(ObservationViewModel observationViewModel);

        Task<ResponseResult> Update(ObservationViewModel observationViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
