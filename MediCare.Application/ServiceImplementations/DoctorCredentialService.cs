using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.CredentialsDTO;
using MediCare.Application.DTOs.LabTestDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class DoctorCredentialService:IDoctorCredentialService
    {
        private readonly IGenericRepository<DoctorCredential> _docCredRepo;

        public DoctorCredentialService(IGenericRepository<DoctorCredential> docCredRepo)
        {
            _docCredRepo = docCredRepo;
        }

       public async Task<ApiResponse<IEnumerable<DoctorCredentialDTO>>> GetAllAsync()
        {
            var result = await _docCredRepo.GetAllAsync("SP_DOC_CREDENTIALS");
            var dtoList = result.Select(labTest => new DoctorCredentialDTO
            {
                Id=labTest.Id,
                DoctorId=labTest.DoctorId,
                CredentialType=labTest.CredentialType,
                DegreeType=labTest.DegreeType,
                DegreeName=labTest.DegreeName,
                InstitutionName=labTest.InstitutionName,
                HospitalName=labTest.HospitalName,
                Designation=labTest.Designation,
                DocumentType=labTest.DocumentType,
                UploadDocument=labTest.UploadDocument,
                StartDate=labTest.StartDate,
                EndDate=labTest.EndDate

            }).ToList();

            return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(200, "DoctorCredentials fetched successfully",dtoList);

        }

        public async Task<ApiResponse<DoctorCredentialDTO>> GetByIdAsync(int id) 
        {
            var result = (await _docCredRepo.GetAllAsync("SP_DOC_CREDENTIALS"))
                                 .FirstOrDefault(t => t.Id == id);

            if (result == null)
                return new ApiResponse<DoctorCredentialDTO>(404, "Doctor Credential not found");
            var dto = new DoctorCredentialDTO
            {
                Id = id,
                DoctorId = result.DoctorId,
                CredentialType = result.CredentialType,
                DegreeType = result.DegreeType,
                DegreeName = result.DegreeName,
                InstitutionName = result.InstitutionName,
                HospitalName = result.HospitalName,
                Designation = result.Designation,
                DocumentType = result.DocumentType,
                UploadDocument = result.UploadDocument,
                StartDate = result.StartDate, // Convert DateOnly → DateTime
                EndDate = result.EndDate
            };


            return new ApiResponse<DoctorCredentialDTO>(200, "Doctor Credential fetched successfully", dto);
        }

        public async Task<ApiResponse<bool>> AddAsync(AddDoctorCredentialDTO dto)
        {
            var parameters = new DoctorCredential
            { 
                DoctorId=dto.DoctorId,
                CredentialType=dto.CredentialType,
                DegreeType=dto.DegreeType,
                DegreeName=dto.DegreeName,
                InstitutionName=dto.InstitutionName,
                HospitalName=dto.HospitalName,
                Designation=dto.Designation,
                DocumentType=dto.DocumentType,
                UploadDocument=dto.UploadDocument,
                StartDate=dto.StartDate,
                EndDate=dto.EndDate                
            };

            var result = await _docCredRepo.AddAsync("SP_DOC_CREDENTIALS", parameters);

            return new ApiResponse<bool>(result > 0 ? 200 : 400,
                                         result > 0 ? "Doctor Credential Added  Successfully" : "Failed to add Doctor Credential",
                                         result > 0);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(UpdateDoctorCredentialDTO dto)
        {
            try
            {
                // Check if the record exists inside this method
                var existingRecord = await _docCredRepo.GetByIdAsync("SP_DOC_CREDENTIALS", "@ID", dto.Id);
                if (existingRecord == null)
                {
                    return new ApiResponse<bool>(404, "Doctor credential not found", false);
                }

                // Map DTO to entity
                var entity = new DoctorCredential
                {
                    Id=dto.Id,
                    CredentialType = dto.CredentialType,
                    DegreeType = dto.DegreeType,
                    DegreeName = dto.DegreeName,
                    InstitutionName = dto.InstitutionName,
                    HospitalName = dto.HospitalName,
                    Designation = dto.Designation,
                    DocumentType = dto.DocumentType,
                    UploadDocument = dto.UploadDocument,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate

                };

                // Call generic repository update
                int rowsAffected = await _docCredRepo.UpdateAsync("SP_DOC_CREDENTIALS", entity);

                if (rowsAffected > 0)
                    return new ApiResponse<bool>(200, "Updated successfully", true);
                else
                    return new ApiResponse<bool>(400, "Update failed", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, $"Error: {ex.Message}", false);
            }


        }
    }
}
