using MediCare.Application.Common;
using MediCare.Application.DTOs.DepartmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IDepartmentService
    {
        Task<ApiResponse<IEnumerable<DepartmentDTO>>> GetAllDepartmentAsync();
        Task<ApiResponse<bool>>AddDepartmentAsync( AddDepartmentDTO addDepartment);
    }
}
