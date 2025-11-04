using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class LabTestService:ILabTestService
    {
        private readonly IGenericRepository<LabTests> _labTestRepo;

        public LabTestService(IGenericRepository<LabTests> labTestRepo)
        {
            _labTestRepo = labTestRepo;
        }

        public async Task<ApiResponse<IEnumerable<LabTestDTO>>> GetAllLabTestsAsync()
        {
            var result = await _labTestRepo.GetAllAsync("SP_LABTEST");
            var dtoList = result.Select(labTest => new LabTestDTO
            {
                LabTestId = labTest.LabTestId,
                TestName = labTest.TestName,
                DepartmentId = labTest.DepartmentId,
                Price = labTest.Price,
                SlotsPerDay = labTest.SlotsPerDay,
                Descriptions = labTest.Descriptions,
                TestCompletionTime = labTest.TestCompletionTime,
                IsActive = labTest.IsActive
            }).ToList();

            return new ApiResponse<IEnumerable<LabTestDTO>>(200, "Lab tests fetched successfully", dtoList);
        }

        public async Task<ApiResponse<LabTestDTO>> GetLabTestByIdAsync(int labTestId)
        {
            var result = (await _labTestRepo.GetAllAsync("SP_LABTEST")).FirstOrDefault(t => t.LabTestId == labTestId);

            if (result == null)
                return new ApiResponse<LabTestDTO>(404, "Lab test not found");
            var dto = new LabTestDTO
            {
                LabTestId = labTestId,
                DepartmentId = result.DepartmentId,
                TestName = result.TestName,
                Price = result.Price,
                SlotsPerDay = result.SlotsPerDay,
                Descriptions = result.Descriptions,
                TestCompletionTime = result.TestCompletionTime,
               


            };


            return new ApiResponse<LabTestDTO>(200, "Lab test fetched successfully", dto);
        }

        public async Task<ApiResponse<bool>> AddLabTestAsync(AddLabTestDTO dto)
        {
            var parameters = new LabTests
            {
               
                TestName= dto.TestName,
                Descriptions= dto.Descriptions,
                Price= dto.Price,
                SlotsPerDay= dto.SlotsPerDay,
                TestCompletionTime= dto.TestCompletionTime,
                DepartmentId= dto.DepartmentId
                
            };

            var result = await _labTestRepo.AddAsync("SP_LABTEST", parameters);

            return new ApiResponse<bool>(result > 0 ? 200 : 400,
                                         result > 0 ? "Lab test added successfully" : "Failed to add lab test",
                                         result > 0);
        }

        public async Task<ApiResponse<bool>> DeleteLabTestAsync(int labTestId, int deletedBy)
        {
            // Since your SP requires DELETEDBY, you need to pass it as part of the ID object
            var idObject = new
            {
                LABTESTID = labTestId,
                DELETEDBY = deletedBy
            };

            var result = await _labTestRepo.DeleteAsync("SP_LABTEST", "@LABTESTID", idObject);

            return new ApiResponse<bool>(
                result > 0 ? 200 : 400,
                result > 0 ? "Lab test deleted successfully" : "Failed to delete lab test",
                result > 0
            );
        }
        
      
    }
}

