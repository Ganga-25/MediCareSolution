using MediCare.Application.Common;
using MediCare.Application.DTOs.LabTestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface ILabTestService
    {
        Task<ApiResponse<IEnumerable<LabTestDTO>>> GetAllLabTestsAsync();
        Task<ApiResponse<LabTestDTO>> GetLabTestByIdAsync(int labTestId);
        Task<ApiResponse<bool>> AddLabTestAsync(AddLabTestDTO dto);
        Task<ApiResponse<bool>> DeleteLabTestAsync(int labTestId, int deletedBy);
    }
}
