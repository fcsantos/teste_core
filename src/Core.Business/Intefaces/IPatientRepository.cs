using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient> GetWithAddressById(Guid id);

        Task<Patient> GetPatientByUserId(string userId);

        Task<IEnumerable<Patient>> GetPatientIsMailNotSender();

        Task<int> GetAllActiveCount();
    }
} 
