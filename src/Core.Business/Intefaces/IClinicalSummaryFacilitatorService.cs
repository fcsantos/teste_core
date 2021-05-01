using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IClinicalSummaryFacilitatorService : IDisposable
    {
        Task Create(ClinicalSummaryFacilitator clinicalSummaryFacilitator);
        Task Update(ClinicalSummaryFacilitator clinicalSummaryFacilitator);
        Task Delete(Guid id);
    }
}
