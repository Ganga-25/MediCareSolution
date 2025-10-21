using MediCare.Application.DTOs;
using MediCare.Application.DTOs.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterPatientAsync(RegisterRequestDTO request);
        Task<AuthResponse> RegisterDoctorAsync(RegisterRequestDTO request);
        Task<AuthResponse> RegisterLabTechnicianAsync(RegisterRequestDTO request);
    }
}
