using MediCare.Application.Common;
using MediCare.Application.DTOs.LabTestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Service
{
    public interface IPatientLabTestService
    {
         Task<ApiResponse<string>>AddLabtestToPatient(AddPatientLabTestDTO labTestDTO,int userId,string userRole);
        Task<ApiResponse<IEnumerable<LabtestViewDTO>>> GetLabTestforPatient(int userId);
    }
}
