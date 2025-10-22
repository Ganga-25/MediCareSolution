using MediCare.Application.Common;
using MediCare.Application.DTOs.ProfileUpdateDTO;
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
        Task<ApiResponse<bool>> RegisterPatientAsync(PatientUpdateDTO Patdto, int userId);
    }
}
