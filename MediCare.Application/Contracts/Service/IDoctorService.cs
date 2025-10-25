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
        Task<ApiResponse<IEnumerable<DoctorDTO>>> GetAllAsync();
        Task<ApiResponse<DoctorDTO>> GetByIdAsync(int id);
        Task<ApiResponse<bool>> RegisterDoctorAsync( DoctorUpdateDTO Docdto, int userId);
        Task<ApiResponse<IEnumerable<DoctorDTO>>> GetDoctorsByDepartmentAsync(int departmentId);
        Task<ApiResponse<IEnumerable<DoctorDTO>>> GetPendingDoctorsAsync();
        
    }
}
