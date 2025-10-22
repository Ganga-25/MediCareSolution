using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IPatientService
    {
        Task<Patients?> GetPatientByUserIdAsync(int userId);
        Task<int> UpdatePatientProfileAsync(Patients patient);
    }
}
