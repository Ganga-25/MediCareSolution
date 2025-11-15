using MediCare.Application.Common;
using MediCare.Application.DTOs;
using MediCare.Application.DTOs.AuthDTO;
using MediCare.Application.DTOs.UsersDTO;
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

        Task<AuthResponse> LoginAsync(LoginRequestDTO request);
        Task<ApiResponse<List<DoctorInformationDTO>>> GetallDoctors();
        Task<ApiResponse<List<PatientInformationDTO>>> GetAllPatients();
        Task<ApiResponse<List<LabtechnicianInformationDTO>>> getalllabtechnician();
    }
}
