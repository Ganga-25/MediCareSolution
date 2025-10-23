using MediCare.Application.Common;
using MediCare.Application.DTOs.DepartmentDTO;
using MediCare.Application.DTOs.SpecializationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface ISpecializationService
    {
        Task<ApiResponse<IEnumerable<SpecializationDTO>>> GetAllSpecializationAsync();
        Task<ApiResponse<bool>> AddSpecializationAsync(AddSpecializationDTO addSpecialization);
    }
}
