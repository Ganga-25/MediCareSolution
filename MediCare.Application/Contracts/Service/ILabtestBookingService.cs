using MediCare.Application.Common;
using MediCare.Application.DTOs.LabTestBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface ILabtestBookingService
    {
        Task<ApiResponse<string>> BookLabTestAsync(LabTestBookingDTO dto, int userId);
        Task<ApiResponse<string>> CancelBookingAsync(int bookingId, int userId);
    }
}
