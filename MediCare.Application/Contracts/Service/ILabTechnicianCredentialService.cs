using MediCare.Application.Common;
using MediCare.Application.DTOs.CredentialDTO;
using MediCare.Application.DTOs.LabTechnicianDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface ILabTechnicianCredentialService
    {
        //Task<ApiResponse<LabTechnicianCredentialDTO>> GetByIdAsync(int id);
        Task<ApiResponse<string>> Addasync(AddLabtechCredentialDTO dto, int userId);
        
        Task<ApiResponse<IEnumerable<LabTechnicianCredentialDTO>>> GetLabtechnicianCredentialsAsync(int userId);
    }
}
