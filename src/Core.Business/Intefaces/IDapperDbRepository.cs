using Core.Business.Models;
using Core.Business.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IDapperDbRepository
    {
        #region Claims
        Task<IEnumerable<UserClaimsDto>> GetAllUserClaimsByUserId(string userId);
        Task<UserClaimsDto> GetUserClaimsById(int Id);
        Task<IEnumerable<AllUsersDto>> GetAllUsers();
        #endregion

        #region Combos
        Task<IEnumerable<ComboDto>> GetAllClinicalSummaryFacilitator(string type);
        Task<IEnumerable<ComboDto>> GetAllPathologies();
        Task<IEnumerable<ComboDto>> GetAllParentPathologies();
        Task<IEnumerable<ComboDto>> GetAllSpecialties();
        Task<IEnumerable<ComboDto>> GetAllParentSpecialties();
        Task<IEnumerable<ComboDto>> GetAllPatients();
        Task<IEnumerable<ComboDto>> GetAllDoctors();
        Task<IEnumerable<ComboDto>> GetAllInquiries();
        #endregion


        Task<PagedResult<Patient>> GetAllPatientsPaged(int pageSize, int pageIndex, string query = null);
    }
}