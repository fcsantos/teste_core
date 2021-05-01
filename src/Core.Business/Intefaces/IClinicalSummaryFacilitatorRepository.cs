using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IClinicalSummaryFacilitatorRepository : IRepository<ClinicalSummaryFacilitator>
    {
        Task<bool> VerifyClinicalSummaryFacilitatorHasPathology(Guid pathologyId);

        Task<IEnumerable<ClinicalSummaryFacilitator>> GetAllByPathologyId(Guid pathologyId);
    }
}
