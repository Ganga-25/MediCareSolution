using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.DepartmentDTO;
using MediCare.Domain.Entities;
using MediCare.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class DepartmentService : IDepartmentService
    {

        private readonly IGenericRepository<Departments> _departmentRepo;
        private readonly CloudinaryService _cloudinaryService;
        public DepartmentService(IGenericRepository<Departments> departmentRepo, CloudinaryService cloudinaryService)
        {

            _departmentRepo = departmentRepo;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ApiResponse<IEnumerable<DepartmentDTO>>> GetAllDepartmentAsync(string? role)
        {
            try
            {
                var departments = await _departmentRepo.GetAllAsync("SP_DEPARTMENTS");


                // 🔹 Admin can see all departments
                if (role == "Admin")
                {
                    departments = await _departmentRepo.GetAllAsync("SP_DEPARTMENTS");
                }
                else
                {
                    // 🔹 Everyone else (users + guests) see only non-deleted
                    var allDepartments = await _departmentRepo.GetAllAsync("SP_DEPARTMENTS");
                    departments = allDepartments.Where(d => d.IsActive == true);
                }
                var departmentDTOs = departments.Select(d => new DepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    Descriptions = d.Descriptions,
                    DepartmentImageUrl = d.DepartmentImageUrl,
                    IsActive = d.IsActive

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

        public async Task<ApiResponse<DepartmentDTO>> GetDepartmentById(int id)
        {
            try
            {
                var department = await _departmentRepo.GetByIdAsync("SP_DEPARTMENTS", "DEPARTMENTID", id);
                if (department == null) return new ApiResponse<DepartmentDTO>(404, "Department is not found.");
                var dto = new DepartmentDTO
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentName = department.DepartmentName,
                    DepartmentImageUrl = department.DepartmentImageUrl,
                    IsActive = department.IsActive
                };

                return new ApiResponse<DepartmentDTO>(200, "Department fetched Successfully.", dto);
            }
            catch (Exception ex)
            {
              return new ApiResponse<DepartmentDTO>(500, ex.Message );
            }
        }
        public async Task<ApiResponse<bool>> AddDepartmentAsync(AddDepartmentDTO addDepartment)
        {
            try
            {
                addDepartment.DepartmentName = addDepartment.DepartmentName.Trim();
                addDepartment.Descriptions = addDepartment.Descriptions.Trim();
                var department = (await _departmentRepo.GetAllAsync("SP_DEPARTMENTS"))
                                      .SingleOrDefault(d => d.DepartmentName.ToLower() == addDepartment.DepartmentName.ToLower());
                if (department != null) return new ApiResponse<bool>(400, "Department Already Exists.");

                string imageUrl = string.Empty;
                if (addDepartment.DepartmentImageUrl != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(addDepartment.DepartmentImageUrl);
                }

                var departmentEntity = new Departments
                {
                    DepartmentName = addDepartment.DepartmentName,
                    Descriptions = addDepartment.Descriptions,
                    DepartmentImageUrl = imageUrl,

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
        public async Task<ApiResponse<string>> UpdateDepartment(UpdateDepartmentDTO updateDepartment, int userId)
        {
            try
            {
                var department = await _departmentRepo.GetByIdAsync("SP_DEPARTMENTS", "DEPARTMENTID", updateDepartment.DepartmentId);
                if (department == null) return new ApiResponse<string>(404, "Department doesnot exist.");

                string imageUrl = string.Empty;
                if (updateDepartment.DepartmentImageUrl != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(updateDepartment.DepartmentImageUrl);
                }

                var updateddepartment = new Departments
                {
                    DepartmentId = updateDepartment.DepartmentId,
                    DepartmentName = updateDepartment.DepartmentName,
                    Descriptions = updateDepartment.Descriptions,
                    DepartmentImageUrl = imageUrl,
                    IsActive=department.IsActive,
                    ModifiedBy = userId
                };
                await _departmentRepo.UpdateAsync("SP_DEPARTMENTS", updateddepartment);
                return new ApiResponse<string>(200, "Department updated successfully");

            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);

            }
        }
        public async Task<ApiResponse<string>> DeleteDepartmentAsync(int departmentId, int userId)
        {
            try
            {
                var department = await _departmentRepo.GetByIdAsync("SP_DEPARTMENTS", "DEPARTMENTID", departmentId);
                if (department == null)
                    return new ApiResponse<string>(404, "Department not found");

                var result = await _departmentRepo.DeleteAsync("SP_DEPARTMENTS", "DEPARTMENTID", departmentId, userId);

                if (result > 0)
                    return new ApiResponse<string>(200, "Department deleted successfully");
                else
                    return new ApiResponse<string>(400, "Failed to delete department");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, $"An error occurred while deleting department: {ex.Message}");
            }
        }
        public async Task<ApiResponse<string>> ActivateandDeactivateDepartment(int departmentId, int userId)
        {
            try
            {

                var department = await _departmentRepo.GetByIdAsync("SP_DEPARTMENTS", "DEPARTMENTID", departmentId);
                if (department == null) return new ApiResponse<string>(404, "Department doesnot exist.");
                bool newStatus=!department.IsActive;
                var newActive = new Departments
                {

                    DepartmentId = departmentId,
                    DepartmentName=department.DepartmentName,
                    Descriptions = department.Descriptions,
                    DepartmentImageUrl=department.DepartmentImageUrl,
                    IsActive = newStatus

                };
                
                await _departmentRepo.UpdateAsync("SP_DEPARTMENTS", newActive);
                string message = newStatus ? "Department activated successfully" : "Department deactivated successfully";
                return new ApiResponse<string>(200, message);

            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, $"An error occurred while deleting department: {ex.Message}");
            }

        }
    }
}
