using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository
{
    public class PatientAnswersRepository : Repository<PatientAnswers>, IPatientAnswersRepository
    {
        private readonly IUser _user;
        public PatientAnswersRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<PatientAnswers>> GetPatientAnswersByInquiryScheduleId(Guid inquiryScheduleId)
        {
            var AnswersByInquirySchedule = await Db.PatientAnswers.AsNoTracking()
                .Where(m => m.InquiryScheduleId == inquiryScheduleId)
                .ToListAsync();

            return AnswersByInquirySchedule;

        }

    }
}