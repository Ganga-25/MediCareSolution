using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.DepartmentDTO;
using MediCare.Application.DTOs.SpecializationDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class SpecializationService:ISpecializationService
    {
        private readonly IGenericRepository<Specialization> _specializationRepo;
        public SpecializationService(IGenericRepository<Specialization> specializationRepo)
        {

            _specializationRepo = specializationRepo;
        }

        public async Task<ApiResponse<IEnumerable<SpecializationDTO>>> GetAllSpecializationAsync()
        {
            try
            {
                var specialization = await _specializationRepo.GetAllAsync("SP_SPECIALIZATION");

                var SpecializationDTOs = specialization.Select(s => new SpecializationDTO
                {
                   SpecializationName=s.SpecializationName,
                   DepartmentId=s.DepartmentId,
                   SpecializationId=s.SpecializationId,

                   

                }).ToList();


                return new ApiResponse<IEnumerable<SpecializationDTO>>(
                    200,
                    "Departments fetched successfully",
                    SpecializationDTOs
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SpecializationDTO>>(
                    500,
                    ex.Message
                );
            }
        }
        public async Task<ApiResponse<bool>> AddSpecializationAsync(AddSpecializationDTO addSpecialization)
        {
            try
            {
                addSpecialization.SpecializationName = addSpecialization.SpecializationName.Trim();
                var department = (await _specializationRepo.GetAllAsync("SP_SPECIALIZATION"))
                                      .SingleOrDefault(d => d.SpecializationName.ToLower() == addSpecialization.SpecializationName.ToLower());
                if (department != null) return new ApiResponse<bool>(400, "Department Already Exists.");
                var SpecializationEntity = new Specialization
                {
                  SpecializationName=addSpecialization.SpecializationName,
                 DepartmentId=addSpecialization.DepartmentId,

                };

                var result = await _specializationRepo.AddAsync("SP_SPECIALIZATION", SpecializationEntity);

                return new ApiResponse<bool>(
                    result > 0 ? 200 : 400,
                    result > 0 ? "Department added successfully" : "Failed to add department",
                    result > 0
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, ex.Message, false);
            }
        }
    }
}
