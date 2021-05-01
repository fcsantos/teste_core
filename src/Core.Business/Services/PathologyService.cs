using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class PathologyService : BaseService, IPathologyService
    {
        private readonly IPathologyRepository _pathologyRepository;
        private readonly IClinicalSummaryFacilitatorRepository _clinicalSummaryFacilitatorRepository;
        private readonly IUser _user;

        public PathologyService(INotifier notifier,
                                IUser user,
                                IPathologyRepository pathologyRepository,
                                IClinicalSummaryFacilitatorRepository clinicalSummaryFacilitatorRepository) : base(notifier)
        {
            _pathologyRepository = pathologyRepository;
            _clinicalSummaryFacilitatorRepository = clinicalSummaryFacilitatorRepository;
            _user = user;
        }

        public async Task Create(Pathology pathology)
        {
            if (!ExecuteValidation(new PathologyValidation(), pathology)) return;

            if (_pathologyRepository.Search(p => p.Name == pathology.Name).Result.Any())
            {
                Notification("Já existe uma patologia com este nome infomado.");
                return;
            }

            await _pathologyRepository.Create(AuditColumns<Pathology>(pathology, "Create", _user.GetUserId()));
        }

        public async Task Update(Pathology pathology)
        {
            if (!ExecuteValidation(new PathologyValidation(), pathology)) return;

            if (_pathologyRepository.Search(p => p.Name == pathology.Name && p.Id != pathology.Id).Result.Any())
            {
                Notification("Já existe uma patologia com este nome infomado.");
                return;
            }

            await _pathologyRepository.Update(AuditColumns<Pathology>(pathology, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            if (await _clinicalSummaryFacilitatorRepository.VerifyClinicalSummaryFacilitatorHasPathology(id))
            {
                Notification("A patologia possui facilitadores cadastrados");
                return;
            }
            else if (await _pathologyRepository.VerifyPathologyHasParentPathology(id))
            {
                Notification("A patologia possui sub patologias cadastrados");
                return;
            }
            
            await _pathologyRepository.Delete(id);
        }

        public void Dispose()
        {
            _pathologyRepository?.Dispose();
            _clinicalSummaryFacilitatorRepository?.Dispose();
        }
    }
}
