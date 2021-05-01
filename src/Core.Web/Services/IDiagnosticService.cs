using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IDiagnosticService
    {
        Task<IEnumerable<DiagnosticViewModel>> GetAll(Guid id);

        Task<IEnumerable<DiagnosticViewModel>> GetAllExceptDoctor(Guid id);

        Task<DiagnosticViewModel> GetById(Guid id);

        Task<ResponseResult> Create(DiagnosticViewModel diagnosticViewModel);

        Task<ResponseResult> Update(DiagnosticViewModel diagnosticViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
