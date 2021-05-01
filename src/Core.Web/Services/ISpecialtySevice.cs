using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface ISpecialtySevice
    {
        Task<IEnumerable<SpecialtyViewModel>> GetAll();

        Task<IEnumerable<SpecialtyViewModel>> GetAllParentSpecialities();

        Task<SpecialtyViewModel> GetById(Guid id);

        Task<ResponseResult> Create(SpecialtyViewModel specialtyViewModel);

        Task<ResponseResult> Update(SpecialtyViewModel specialtyViewModel);

        Task<ResponseResult> Delete(Guid id);


        Task<IEnumerable<ComboViewModel>> ComboParentSpecialties();
        Task<IEnumerable<ComboViewModel>> ComboSpecialties();
    }
}
