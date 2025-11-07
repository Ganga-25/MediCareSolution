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

        public async Task<ApiResponse<IEnumerable<LabTestDTO>>> GetAllLabTestsAsync(string? role)
        {
            try
            {
                var labtests = await _labTestRepo.GetAllAsync("SP_LABTEST");
                if (role == "Admin")
                {
                    labtests = await _labTestRepo.GetAllAsync("SP_LABTEST");
                }
                else
                {
                    labtests = await _labTestRepo.GetAllAsync("SP_LABTEST");
                    labtests = labtests.Where(l => l.IsActive = true);
                }


                var dtoList = labtests.Select(labTest => new LabTestDTO
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
            catch (Exception ex)
            {
               return new ApiResponse<IEnumerable<LabTestDTO>>(500, ex.Message);
            }
           
        }

        public async Task<ApiResponse<LabTestDTO>> GetLabTestByIdAsync(int labTestId)
        {
            try
            {
                var result = await _labTestRepo.GetByIdAsync("SP_LABTEST", "LABTESTID", labTestId);

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
                    IsActive=result.IsActive
                };

                return new ApiResponse<LabTestDTO>(200, "Lab test fetched successfully", dto);
            }
            catch (Exception ex)
            {
                 return new ApiResponse<LabTestDTO>(500, ex.Message);
            }
           
        }

        public async Task<ApiResponse<string>> AddLabTestAsync(AddLabTestDTO dto)
        {
            try
            {
                var labtestName=dto.TestName.Trim();
                var labtests =( await _labTestRepo.GetAllAsync("SP_LABTEST")).SingleOrDefault(l=>l.TestName.Trim()==labtestName);
                if (labtests != null) return new ApiResponse<string>(400, "The labtest already exists");
                var parameters = new LabTests
                {

                    TestName = dto.TestName,
                    Descriptions = dto.Descriptions,
                    Price = dto.Price,
                    SlotsPerDay = dto.SlotsPerDay,
                    TestCompletionTime = dto.TestCompletionTime,
                    DepartmentId = dto.DepartmentId
                   
                };

                var result = await _labTestRepo.AddAsync("SP_LABTEST", parameters);

                return new ApiResponse<string>(200, "Labtest Added Successfully.");


            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);

            }
            
        }

        public async Task<ApiResponse<string>> UpdateLabTest(UpdateLabTestDTO testDTO, int userid)
        {
            try
            {
                var labtests = await _labTestRepo.GetByIdAsync("SP_LABTEST", "LABTESTID", testDTO.LabTestId);
                if (labtests == null) return new ApiResponse<string>(400, "Labtest not found");

                var updatedTest = new LabTests
                {
                    LabTestId = testDTO.LabTestId,
                    TestName = testDTO.TestName ?? labtests.TestName,
                    DepartmentId = testDTO.DepartmentId ?? labtests.DepartmentId,
                    Descriptions = testDTO.Descriptions ?? labtests.Descriptions,
                    Price = testDTO.Price ?? labtests.Price,
                    SlotsPerDay = testDTO.SlotsPerDay ?? labtests.SlotsPerDay,
                    TestCompletionTime = testDTO.TestCompletionTime ?? labtests.TestCompletionTime,
                    ModifiedBy = userid
                };

                await _labTestRepo.UpdateAsync("SP_LABTEST", updatedTest);
                return new ApiResponse<string>(200, "Labtest Updated Successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500,ex.Message);
            }
        }
        public async Task<ApiResponse<string>> ActivateorDeactivaeLabtest(int labtestid, int userid)
        {
            try
            {
                var labtests = await _labTestRepo.GetByIdAsync("SP_LABTEST", "LABTESTID",labtestid);
                if (labtests == null) return new ApiResponse<string>(400, "Labtest not found");

                bool newStatus = !labtests.IsActive;
                var newActiveStatus = new LabTests
                {
                    LabTestId = labtests.LabTestId,
                    DepartmentId = labtests.DepartmentId,
                    Descriptions = labtests.Descriptions,
                    Price = labtests.Price,
                    SlotsPerDay = labtests.SlotsPerDay,
                    TestCompletionTime = labtests.TestCompletionTime,
                    IsActive = newStatus,
                    ModifiedBy = userid
                };
                await _labTestRepo.UpdateAsync("SP_LABTEST", newActiveStatus);
                string message = newStatus ? "Department activated successfully" : "Department deactivated successfully";
                return new ApiResponse<string>(200, message);
              
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500,ex.Message);
            }
        }



    }
}

