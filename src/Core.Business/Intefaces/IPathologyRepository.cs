using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IPathologyRepository : IRepository<Pathology>
    {
        Task<IEnumerable<Pathology>> GetSubPathologiesPathologies();
        Task<Pathology> GetSubPathologiesByPathologyId(Guid pathologyId);
        Task<IEnumerable<Pathology>> GetAllParentPathologies();
        Task<bool> VerifyPathologyHasParentPathology(Guid parentId);
    }
}
