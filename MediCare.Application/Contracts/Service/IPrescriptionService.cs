using MediCare.Application.Common;
using MediCare.Application.DTOs.PrescriptionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IPrescriptionService
    {
        Task<ApiResponse<string>> AddPrescription(AddPresciptionDTO addPresciptionDTO, int doctorId, string UserRole);
       
    }
}
