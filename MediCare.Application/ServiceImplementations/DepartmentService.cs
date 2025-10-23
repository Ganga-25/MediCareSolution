using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.DepartmentDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class DepartmentService:IDepartmentService
    {
        
        private readonly IGenericRepository<Departments> _departmentRepo;
        public DepartmentService( IGenericRepository<Departments> departmentRepo)
        {
            
            _departmentRepo=departmentRepo;
        }

        public async Task<ApiResponse<IEnumerable<DepartmentDTO>>> GetAllDepartmentAsync()
        {
            try
            {
                var departments = await _departmentRepo.GetAllAsync("SP_DEPARTMENTS");

                var departmentDTOs = departments.Select(d => new DepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    Descriptions = d.Descriptions,
                    DepartmentImageUrl = d.DepartmentImageUrl,
                    
                }).ToList();


                return new ApiResponse<IEnumerable<DepartmentDTO>>(
                    200,
                    "Departments fetched successfully",
                    departmentDTOs
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DepartmentDTO>>(
                    500,
                    ex.Message
                );
            }
        }
        public async Task<ApiResponse<bool>> AddDepartmentAsync(AddDepartmentDTO addDepartment)
        {
            try
            {
                addDepartment.DepartmentName=addDepartment.DepartmentName.Trim();
                addDepartment.Descriptions=addDepartment.Descriptions.Trim();
                var department=(await _departmentRepo.GetAllAsync("SP_DEPARTMENTS"))
                                      .SingleOrDefault(d=>d.DepartmentName.ToLower()==addDepartment.DepartmentName.ToLower());
                if (department != null) return new ApiResponse<bool>(400, "Department Already Exists.");
                var departmentEntity = new Departments
                {
                    DepartmentName = addDepartment.DepartmentName,
                    Descriptions = addDepartment.Descriptions,
                    DepartmentImageUrl = addDepartment.DepartmentImageUrl,
                    
                };

                var result = await _departmentRepo.AddAsync("SP_DEPARTMENTS", departmentEntity);

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
