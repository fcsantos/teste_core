using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IPathologyService
    {
        Task<IEnumerable<PathologyViewModel>> GetAll();

        Task<IEnumerable<PathologyViewModel>> GetAllParentPathologies();

        Task<PathologyViewModel> GetById(Guid id);

        Task<ResponseResult> Create(PathologyViewModel pathologyViewModel);

        Task<ResponseResult> Update(PathologyViewModel pathologyViewModel);

        Task<ResponseResult> Delete(Guid id);

        Task<IEnumerable<ComboViewModel>> ComboPathologies();
        Task<IEnumerable<ComboViewModel>> ComboParentPathologies();
    }
}
