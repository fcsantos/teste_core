using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientViewModel>> GetAll();

        Task<PatientViewModel> GetById(Guid id);

        Task<PatientViewModel> GetByUserId(Guid id);

        Task<ResponseResult> Create(PatientViewModel patientViewModel);

        Task<ResponseResult> Update(PatientViewModel patientViewModel);

        Task<ResponseResult> Delete(Guid id);

        Task<ResponseResult> DeleteUser(string id);

        Task<PagedViewModel<PatientViewModel>> GetAllPatientsPaged(int pageSize, int pageIndex, string query = null);

        Task<IEnumerable<ComboViewModel>> Combo();

    }
}
