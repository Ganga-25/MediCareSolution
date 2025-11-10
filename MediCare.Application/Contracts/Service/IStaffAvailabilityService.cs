using MediCare.Application.Common;
using MediCare.Application.DTOs.AvailabilityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IStaffAvailabilityService
    {
        Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetByIdAsync(int id);
        Task<ApiResponse<bool>> AddDoctorAvailability(StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role);
        Task<ApiResponse<bool>> AddLabtechnicianAvailability(StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role);
        Task<ApiResponse<bool>> UpdateAsync( StaffAvailabilityCreateUpdateDTO dto, int id, string role);
        Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetByStaffIdAsync(int staffId);

    }
}
