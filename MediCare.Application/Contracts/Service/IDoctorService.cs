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
    public interface IDoctorService
    {
        //Task<ApiResponse<DoctorDTO>> GetDoctorByUserIdAsync(int userId);
        //Task<int> UpdateDoctorProfileAsync( labTech);
        Task<ApiResponse<bool>> RegisterDoctorAsync( DoctorUpdateDTO Docdto, int userId);
    }
}
