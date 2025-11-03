using MediCare.Application.Common;
using MediCare.Application.DTOs.AppointmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IAppointmentService
    {
        Task<ApiResponse<string>> BookAppointmentAsync(AppointmentDTO dto,int userId);
        Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAllAppointmentsAsync();
        Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAppointmentsByPatientIdAsync(int userId);
        Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAppointmentsByDoctorIdAsync(int doctorId);

        Task<ApiResponse<string>> CancelAppointmentAsync(CancelAppointmentDTO dto, int userId, string role);
        Task<ApiResponse<string>> RescheduleAppointmentAsync(RescheduleAppointmentDTO dto, int userId, string role);

    }
}
