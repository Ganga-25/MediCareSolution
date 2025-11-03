using MediCare.Application.Common;
using MediCare.Application.DTOs.CredentialDTO;
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
        Task<ApiResponse<bool>> AddAsync(AddDoctorCredentialDTO dto,int userId);
        Task<ApiResponse<bool>> UpdateAsync(UpdateDoctorCredentialDTO dto);
        Task<ApiResponse<IEnumerable<DoctorCredentialDTO>>> GetDoctorCredentialsAsync(int userId);
    }
}
