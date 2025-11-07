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
        Task<ApiResponse<IEnumerable<LabTestDTO>>> GetAllLabTestsAsync(string? role);
        Task<ApiResponse<LabTestDTO>> GetLabTestByIdAsync(int labTestId);
        Task<ApiResponse<string>> AddLabTestAsync(AddLabTestDTO dto);
        Task<ApiResponse<string>> UpdateLabTest(UpdateLabTestDTO testDTO, int userid);
        Task<ApiResponse<string>> ActivateorDeactivaeLabtest(int labtestid,int userid);
      
        
    }
}
