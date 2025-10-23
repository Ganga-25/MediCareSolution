using MediCare.Application.Common;
using MediCare.Application.DTOs.CredentialsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IDoctorCredentialService
    {
        Task< ApiResponse<IEnumerable<DoctorCredentialDTO>>> GetAllAsync();
        Task<ApiResponse<DoctorCredentialDTO>> GetByIdAsync(int id);
        Task<ApiResponse<bool>> AddAsync(AddDoctorCredentialDTO dto);
        Task<ApiResponse<bool>> UpdateAsync(UpdateDoctorCredentialDTO dto);
    }
}
