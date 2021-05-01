using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IAllergyService
    {
        Task<IEnumerable<AllergyViewModel>> GetAll(Guid id);

        Task<IEnumerable<AllergyViewModel>> GetAllExceptDoctor(Guid id);

        Task<AllergyViewModel> GetById(Guid id);

        Task<ResponseResult> Create(AllergyViewModel allergyViewModel);

        Task<ResponseResult> Update(AllergyViewModel allergyViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
