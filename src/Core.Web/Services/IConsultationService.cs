using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IConsultationService
    {
        Task<IEnumerable<ConsultationViewModel>> GetAll(Guid id);

        Task<IEnumerable<ConsultationViewModel>> GetAllExceptDoctor(Guid id);

        Task<ConsultationViewModel> GetById(Guid id);

        Task<ResponseResult> Create(ConsultationViewModel consultationViewModel);

        Task<ResponseResult> Update(ConsultationViewModel consultationViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
