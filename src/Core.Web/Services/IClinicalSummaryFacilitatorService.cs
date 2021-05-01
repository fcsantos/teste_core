using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IClinicalSummaryFacilitatorService
    {
        Task<IEnumerable<ClinicalSummaryFacilitatorViewModel>> GetAll();

        Task<ClinicalSummaryFacilitatorViewModel> GetById(Guid id);

        Task<IEnumerable<ClinicalSummaryFacilitatorViewModel>> GetAllByPathologyId(Guid id);

        Task<ResponseResult> Create(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel);

        Task<ResponseResult> Update(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel);

        Task<ResponseResult> Delete(Guid id);


        Task<IEnumerable<ComboViewModel>> Combo(string type);
    }
}
