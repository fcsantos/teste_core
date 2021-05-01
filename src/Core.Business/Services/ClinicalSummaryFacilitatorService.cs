using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class ClinicalSummaryFacilitatorService : BaseService, IClinicalSummaryFacilitatorService
    {
        private readonly IClinicalSummaryFacilitatorRepository _clinicalSummaryFacilitatorRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public ClinicalSummaryFacilitatorService(IClinicalSummaryFacilitatorRepository clinicalSummaryFacilitatorRepository,
                                                 IDoctorRepository doctorRepository,
                                                 INotifier notifier, IUser user) : base(notifier)
        {
            _clinicalSummaryFacilitatorRepository = clinicalSummaryFacilitatorRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(ClinicalSummaryFacilitator clinicalSummaryFacilitator)
        {
            if (!ExecuteValidation(new ClinicalSummaryFacilitatorValidation(), clinicalSummaryFacilitator)) return;

            if (_clinicalSummaryFacilitatorRepository.Search(c => c.Name == clinicalSummaryFacilitator.Name && 
                                                                  c.Description == clinicalSummaryFacilitator.Description && 
                                                                  c.PathologyId == clinicalSummaryFacilitator.PathologyId).Result.Any())
            {
                Notification("Já existe um facilitador com este nome e descrição infomados para a patologia escolhida.");
                return;
            }

            clinicalSummaryFacilitator.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;

            await _clinicalSummaryFacilitatorRepository.Create(AuditColumns<ClinicalSummaryFacilitator>(clinicalSummaryFacilitator, "Create", _user.GetUserId()));
        }

        public async Task Update(ClinicalSummaryFacilitator clinicalSummaryFacilitator)
        {
            if (!ExecuteValidation(new ClinicalSummaryFacilitatorValidation(), clinicalSummaryFacilitator)) return;

            if (_clinicalSummaryFacilitatorRepository.Search(c => c.Name == clinicalSummaryFacilitator.Name &&
                                                                  c.Description == clinicalSummaryFacilitator.Description &&
                                                                  c.PathologyId == clinicalSummaryFacilitator.PathologyId &&
                                                                  c.Id != clinicalSummaryFacilitator.Id).Result.Any())
            {
                Notification("Já existe um facilitador com este nome e descrição infomados para a patologia escolhida.");
                return;
            }

            await _clinicalSummaryFacilitatorRepository.Update(AuditColumns<ClinicalSummaryFacilitator>(clinicalSummaryFacilitator, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _clinicalSummaryFacilitatorRepository.Delete(id);
        }

        public void Dispose()
        {
            _clinicalSummaryFacilitatorRepository?.Dispose();
        }
    }
}
