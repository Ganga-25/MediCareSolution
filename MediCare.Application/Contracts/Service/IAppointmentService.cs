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
        Task<ApiResponse<string>> BookAppointmentAsync(AppointmentDTO dto);
    }
}
