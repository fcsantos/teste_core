using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository
{
    public class InquiryScheduleRepository : Repository<InquirySchedule>, IInquiryScheduleRepository
    {
        private readonly IUser _user;
        public InquiryScheduleRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<InquirySchedule>> GetAllByDoctorUserId()
        {
            var doctorId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            var byDoctor = await Db.InquiriesSchedule.AsNoTracking()
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .Where(m => m.DoctorId == doctorId)
                .OrderBy(m => m.StartDate)
                .ToListAsync();
            return byDoctor;
        }

        public async Task<IEnumerable<InquirySchedule>> GetAllByPatientUserId()
        {
            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            var byPatient = await Db.InquiriesSchedule.AsNoTracking()
                .Include(c => c.Inquiry)
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .Where(m => m.PatientId == patientId)
                .OrderBy(m => m.StartDate)
                .ToListAsync();
            return byPatient;

        }

        public async Task<IEnumerable<InquirySchedule>> GetAllByPatientId(Guid id)
        {
            var byPatient = await Db.InquiriesSchedule.AsNoTracking()
                .Include(c => c.Inquiry)
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .Where(m => m.PatientId == id)
                .OrderBy(m => m.StartDate)
                .ToListAsync();
            return byPatient;

        }

        public async Task<IEnumerable<InquirySchedule>> GetAllByPatientAnsweredOrNot(bool answered)
        {

            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            var byPatientAnswerdOrNor = await Db.InquiriesSchedule.AsNoTracking()
                .Include(c => c.Inquiry)
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .Where(m => m.PatientId == patientId && m.Answered == answered )
                .OrderBy(m => m.StartDate)
                .ToListAsync();
            return byPatientAnswerdOrNor;

        }

        public async Task<IEnumerable<InquirySchedule>> GetAllByAnsweredOrNot(bool answered)
        {

            var byStatus = await Db.InquiriesSchedule.AsNoTracking()
           .Include(c => c.Patient)
           .Include(c => c.Doctor)
           .Include(c => c.Inquiry)
           .Where(m => m.Answered == answered)
           .OrderBy(m => m.StartDate)
           .ToListAsync();
            return byStatus;

        }

        public async Task<IEnumerable<InquirySchedule>> GetAllInquiryScheduleIsMailNotSender()
        {
            return await Db.InquiriesSchedule.AsNoTracking().Include(isc => isc.Patient).Include(isc => isc.Doctor).Where(p => !p.IsMailSender).ToListAsync();
        }
    }
}