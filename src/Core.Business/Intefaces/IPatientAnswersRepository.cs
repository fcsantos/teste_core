using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IPatientAnswersRepository : IRepository<PatientAnswers>
    {
        //Task<IEnumerable<PatientAnswers>> GetAllPatientAnswersByPatient(Guid patientId);
        Task<IEnumerable<PatientAnswers>> GetPatientAnswersByInquiryScheduleId(Guid inquiryScheduleId);

    }
}