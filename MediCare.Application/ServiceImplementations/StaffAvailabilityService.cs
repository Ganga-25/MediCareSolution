using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AvailabilityDTO;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class StaffAvailabilityService : IStaffAvailabilityService
    {
        private readonly IGenericRepository<StaffAvailability> _repository;
        private readonly IGenericRepository<Doctors> _doctorRepo;
        private readonly IGenericRepository<LabTechnicians> _LabtechRepo;

        public StaffAvailabilityService(IGenericRepository<StaffAvailability> repository, IGenericRepository<Doctors>doctorRepo,IGenericRepository<LabTechnicians>labtechRepo)
        {
            _repository = repository;
            _doctorRepo = doctorRepo;
            _LabtechRepo = labtechRepo;
        }

        public async Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync("SP_STAFF_AVAILABILITY");
            //return result.Select(x => MapToDto(x));
            var Staffs = result.Select(x => new StaffAvailabilityDTO
            {
                Id = x.Id,
                StaffType = x.StaffType,
                StaffId = x.StaffId,
                Date = x.Date,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                SlotDuration = x.SlotDuration,
                Mode = x.Mode,
                SlotList = x.SlotList,
                Session = x.Session,
                AvailableSlots = x.AvailableSlots
            }).ToList();
            return new ApiResponse<IEnumerable<StaffAvailabilityDTO>>(200, "DoctorAvailability Fetched Successfully", Staffs);

        }

        public async Task<ApiResponse<StaffAvailabilityDTO>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync("SP_STAFF_AVAILABILITY", "@ID", id);
            var Staff = new StaffAvailabilityDTO
            {
                Id = entity.Id,
                StaffType = entity.StaffType,
                StaffId = entity.StaffId,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                SlotDuration = entity.SlotDuration,
                Mode = entity.Mode,
                SlotList = entity.SlotList,
                Session = entity.Session,
                AvailableSlots = entity.AvailableSlots

            };
            return new ApiResponse<StaffAvailabilityDTO>(200, "Slots for doctor identified", Staff);

        }

        public async Task<ApiResponse<bool>> AddAsync(StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role)
        {
            try
            {
                int staffId=0;
                StaffType staffType=StaffType.Doctor;

                if (role == "Doctor")
                {
                    var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                    var doctor = doctors.SingleOrDefault(x => x.UserId == currentUserId);
                    if (doctor == null) return new ApiResponse<bool>(404, "Doctor not found");

                    staffId = doctor.DoctorId;
                    staffType = StaffType.Doctor;

                }else if(role == "Labtechnician")
                {

                    var LabTechnicians = await _LabtechRepo.GetAllAsync("SP_LABTECH");
                    var labtech = LabTechnicians.FirstOrDefault(x => x.UserId == currentUserId);
                    if (labtech == null) return new ApiResponse<bool>(404, "Labtechnician not found");

                    staffId = labtech.LabTechnicianId;
                    staffType = StaffType.LabTechnicia;

                }

                    var newStaff = new StaffAvailability
                    {
                        StaffType =staffType,
                        StaffId = staffId,
                        Date = dto.Date,
                        StartTime = dto.StartTime,
                        EndTime = dto.EndTime,
                        SlotDuration = dto.SlotDuration,
                        Mode = dto.Mode,
                        SlotList = dto.SlotList,
                        Session = dto.Session,
                        AvailableSlots = dto.AvailableSlots
                    };

                
                var result = await _repository.AddAsync("SP_STAFF_AVAILABILITY", newStaff);

                return new ApiResponse<bool>(
                    statuscode: result > 0 ? 200 : 400,
                    message: result > 0 ? "Staff availability added successfully" : "Failed to add staff availability",
                    data: result > 0
                );


            }
            catch (Exception ex)
            {
                 return new ApiResponse<bool>(500,ex.Message);
            }


        }


        public async Task<ApiResponse<bool>> UpdateAsync(int id, StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role)
        {
            try
            {
                int staffId = 0;
                StaffType staffType = StaffType.Doctor;

                if (role == "Doctor")
                {
                    var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                    var doctor = doctors.SingleOrDefault(x => x.UserId == currentUserId);
                    if (doctor == null) return new ApiResponse<bool>(404, "Doctor not found");

                    staffId = doctor.DoctorId;
                    staffType = StaffType.Doctor;

                }
                else if (role == "Labtechnician")
                {

                    var LabTechnicians = await _LabtechRepo.GetAllAsync("SP_LABTECH");
                    var labtech = LabTechnicians.FirstOrDefault(x => x.UserId == currentUserId);
                    if (labtech == null) return new ApiResponse<bool>(404, "Labtechnician not found");

                    staffId = labtech.LabTechnicianId;
                    staffType = StaffType.LabTechnicia;

                }
                var entity = new StaffAvailability
                {
                    StaffId = staffId,
                    StaffType = staffType,
                    Date = dto.Date,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    AvailableSlots = dto.AvailableSlots,
                    SlotDuration = dto.SlotDuration,
                    Mode = dto.Mode,
                    Session = dto.Session,
                    SlotList = dto.SlotList,
                    ModifiedBy=currentUserId
                    

                };
                await _repository.UpdateAsync("SP_STAFF_AVAILABILITY", entity);
                return new ApiResponse<bool>(201, "StaffAvailability updated successfully.");


            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, ex.Message);
            }

            
         
        }

     

   
        public async Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetByStaffIdAsync(int staffId)
        {
            var result = (await _repository.GetAllAsync("SP_STAFF_AVAILABILITY"))
                                   .Where(x => x.StaffId == staffId);
            var staffAvailabilityList = result.Select(x => new StaffAvailabilityDTO
            {
                Id = x.Id,
                StaffType = x.StaffType,
                StaffId = x.StaffId,
                Date = x.Date,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                AvailableSlots = x.AvailableSlots,
                SlotDuration = x.SlotDuration,
                Mode = x.Mode,
                Session = x.Session,
                SlotList = x.SlotList
            }).ToList();
            return new ApiResponse<IEnumerable<StaffAvailabilityDTO>>(200, "Doctor Availability Fetched Successfully.", staffAvailabilityList);

        }
    }
}