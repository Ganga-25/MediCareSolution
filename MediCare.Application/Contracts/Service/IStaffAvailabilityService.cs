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
        Task<ApiResponse<StaffAvailabilityDTO>> GetByIdAsync(int id);
        Task<ApiResponse<bool>> AddAsync(StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role);
        Task<ApiResponse<int>> UpdateAsync(int id, StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role);
        Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetByStaffIdAsync(int staffId);

    }
}
