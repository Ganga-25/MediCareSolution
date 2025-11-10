using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.CredentialDTO;
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
        private readonly IGenericRepository<Doctors> _doctorRepo;

        public DoctorCredentialService(IGenericRepository<DoctorCredential> docCredRepo,IGenericRepository<Doctors>doctorRepo)
        {
            _docCredRepo = docCredRepo;
            _doctorRepo = doctorRepo;
        }

       public async Task<ApiResponse<IEnumerable<DoctorCredentialDTO>>> GetAllAsync()
        {
            var result = await _docCredRepo.GetAllAsync("SP_DOC_CREDENTIALS");
            var dtoList = result.Select(docCre => new DoctorCredentialDTO
            {
                Id= docCre.Id,
                DoctorId= docCre.DoctorId,
                CredentialType= docCre.CredentialType,
                DegreeType= docCre.DegreeType,
                DegreeName= docCre.DegreeName,
                InstitutionName= docCre.InstitutionName,
                HospitalName= docCre.HospitalName,
                Designation= docCre.Designation,
                DocumentType= docCre.DocumentType,
                UploadDocument= docCre.UploadDocument,
                StartDate= docCre.StartDate,
                EndDate= docCre.EndDate

            }).ToList();

            return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(200, "DoctorCredentials fetched successfully",dtoList);

        }

        public async Task<ApiResponse<IEnumerable<DoctorCredentialDTO>>> GetByIdAsync(int id) 
        {
            try
            {
                var result = (await _docCredRepo.GetAllAsync("SP_DOC_CREDENTIALS"))
                               .Where(t => t.DoctorId == id).ToList();

                if (result == null)
                    return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(404, "Doctor Credential not found");
                var dto = result.Select(docCre => new DoctorCredentialDTO
                {
                    Id = id,
                    DoctorId = docCre.DoctorId,
                    CredentialType = docCre.CredentialType,
                    DegreeType = docCre.DegreeType,
                    DegreeName = docCre.DegreeName,
                    InstitutionName = docCre.InstitutionName,
                    HospitalName = docCre.HospitalName,
                    Designation = docCre.Designation,
                    DocumentType = docCre.DocumentType,
                    UploadDocument = docCre.UploadDocument,
                    StartDate = docCre.StartDate, // Convert DateOnly → DateTime
                    EndDate = docCre.EndDate
                }).ToList();


                return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(200, "Doctor Credential fetched successfully", dto);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(500, ex.Message);
            }
          
        }

        public async Task<ApiResponse<bool>> AddAsync(AddDoctorCredentialDTO dto,int userId)
        {
            try
            {
                var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                var doctor = doctors.SingleOrDefault(x => x.UserId == userId);
                if (doctor == null) return new ApiResponse<bool>(404, "Doctor not found");

                var parameters = new DoctorCredential
                {
                    DoctorId = doctor.DoctorId,
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

                var result = await _docCredRepo.AddAsync("SP_DOC_CREDENTIALS", parameters);

                return new ApiResponse<bool>(result > 0 ? 200 : 400,
                                             result > 0 ? "Doctor Credential Added  Successfully" : "Failed to add Doctor Credential",
                                              result > 0);


            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, ex.Message);
            }
                                   
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

        public async Task<ApiResponse<IEnumerable<DoctorCredentialDTO>>> GetDoctorCredentialsAsync(int userId)
        {
            // Get all credentials from the repo
            try
            {
                var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                var doctor = doctors.SingleOrDefault(x => x.UserId == userId);
                if (doctor == null) return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(404, "Doctor not found");

                var allCreds = await _docCredRepo.GetAllAsync("SP_DOC_CREDENTIALS");
                var doctorCredentials = allCreds.Where(x => x.DoctorId == doctor.DoctorId);

              
                var dtoList = doctorCredentials.Select(c => new DoctorCredentialDTO
                {
                    Id = c.Id,
                    DoctorId = c.DoctorId,
                    CredentialType = c.CredentialType,
                    DegreeType = c.DegreeType,
                    DegreeName = c.DegreeName,
                    InstitutionName = c.InstitutionName,
                    HospitalName = c.HospitalName,
                    Designation = c.Designation,
                    DocumentType = c.DocumentType,
                    UploadDocument = c.UploadDocument,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                });

                return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(200, "Credentials fetched successfully", dtoList);


            }catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DoctorCredentialDTO>>(500,ex.Message);
            }
         }

       
    }
}
