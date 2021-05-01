using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IPatientAnswersService : IDisposable
    {
        Task Create(PatientAnswers patientAnswers);
        Task Update(PatientAnswers patientAnswers);
        Task Delete(Guid id);
        Task Delete(PatientAnswers patientAnswers);
    } 
}