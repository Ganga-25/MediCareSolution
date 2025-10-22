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
    public interface ILabtechnicianService
    {
        Task<ApiResponse<LabTechnicianDTO>> GetLabTechByUserIdAsync(int userId);
        Task<ApiResponse<bool>> RegisterLabTechnicianAsync(LabTechnicianUpdateDTO labTech, int userId);

    }
}
