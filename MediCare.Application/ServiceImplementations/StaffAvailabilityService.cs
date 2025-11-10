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
            try
            {
                var result = await _repository.GetAllAsync("SP_STAFF_AVAILABILITY");
                
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
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<StaffAvailabilityDTO>>(500, ex.Message);
            }
           

        }

        public async Task<ApiResponse<IEnumerable<StaffAvailabilityDTO>>> GetByIdAsync(int id)
        {
            try
            {
                var entity = (await _repository.GetAllAsync("SP_STAFF_AVAILABILITY")).Where(s => s.StaffId == id);
                var Staff = entity.Select(s => new StaffAvailabilityDTO
                {
                    Id = s.Id,
                    StaffType = s.StaffType,
                    StaffId = s.StaffId,
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    SlotDuration = s.SlotDuration,
                    Mode = s.Mode,
                    SlotList = s.SlotList,
                    Session = s.Session,
                    AvailableSlots = s.AvailableSlots

                }).ToList();

                return new ApiResponse<IEnumerable<StaffAvailabilityDTO>>(200, "Slots for doctor identified", Staff);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<StaffAvailabilityDTO>>(500, ex.Message);
            }
           

        }

        public async Task<ApiResponse<bool>> AddDoctorAvailability(StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role)
        {
            try
            {
                int staffId = 0;
                StaffType staffType = StaffType.Doctor;

                if (role == "Doctor")
                {
                    // ✅ Doctor adds their own availability
                    var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                    var doctor = doctors.SingleOrDefault(x => x.UserId == currentUserId);
                    if (doctor == null)
                        return new ApiResponse<bool>(404, "Doctor not found");

                    staffId = currentUserId;
                    staffType = StaffType.Doctor;
                }
              
                else
                {
                    return new ApiResponse<bool>(403, "You are not authorized to add staff availability");
                }

                TimeSpan startTime, endTime;
                try
                {
                    startTime = DateTime.ParseExact(dto.StartTime.ToString(), "hh:mm tt", null).TimeOfDay;
                    endTime=DateTime.ParseExact(dto.EndTime.ToString(), "hh:mm tt",null).TimeOfDay;

                }
                catch
                {
                    return new ApiResponse<bool>(400, "Invalid appointment time format. Use 'hh:mm tt' (e.g. 06:30 PM).");
                }
                // ✅ Create new record
                var newStaff = new StaffAvailability
                {
                    StaffType = staffType,
                    StaffId = staffId,
                    Date = dto.Date,
                    StartTime = startTime,
                    EndTime = endTime,
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
                return new ApiResponse<bool>(500, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddLabtechnicianAvailability(StaffAvailabilityCreateUpdateDTO dto, int currentUserId, string role)
        {
            try
            {
                if (dto.StaffId == null || dto.StaffId == 0)
                    return new ApiResponse<bool>(400, "Lab technician StaffId is required");

                var labtech = (await _LabtechRepo.GetAllAsync("SP_LABTECH"))
                              .FirstOrDefault(x => x.UserId == dto.StaffId);
                if (labtech == null)
                    return new ApiResponse<bool>(404, "Lab technician not found");

                TimeSpan startTime, endTime;
                try
                {
                    startTime = DateTime.ParseExact(dto.StartTime.ToString(), "hh:mm tt", null).TimeOfDay;
                    endTime = DateTime.ParseExact(dto.EndTime.ToString(), "hh:mm tt", null).TimeOfDay;

                }
                catch
                {
                    return new ApiResponse<bool>(400, "Invalid appointment time format. Use 'hh:mm tt' (e.g. 06:30 PM).");
                }

                var entity = new StaffAvailability
                {
                    StaffType = StaffType.LabTechnician,
                    StaffId = dto.StaffId.Value,
                    Date = dto.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    SlotDuration = dto.SlotDuration,
                    Mode = dto.Mode,
                    SlotList = dto.SlotList,
                    Session = dto.Session,
                    AvailableSlots = dto.AvailableSlots,
                    CreatedBy = role
                };

                var result = await _repository.AddAsync("SP_STAFF_AVAILABILITY", entity);
                return new ApiResponse<bool>(result > 0 ? 200 : 400, result > 0 ? "Lab technician availability added" : "Failed");


            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(StaffAvailabilityCreateUpdateDTO dto, int id, string role)
        {
            try
            {
                int staffId = 0;
                StaffType staffType = StaffType.Doctor;

                if (role == "Doctor")
                {
                    var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                    var doctor = doctors.SingleOrDefault(x => x.UserId == id);
                    if (doctor == null) return new ApiResponse<bool>(404, "Doctor not found");

                    staffId = doctor.UserId;
                    staffType = StaffType.Doctor;

                }
               
                TimeSpan startTime, endTime;
                try
                {
                    startTime = DateTime.ParseExact(dto.StartTime.ToString(), "hh:mm tt", null).TimeOfDay;
                    endTime = DateTime.ParseExact(dto.EndTime.ToString(), "hh:mm tt", null).TimeOfDay;

                }
                catch
                {
                    return new ApiResponse<bool>(400, "Invalid appointment time format. Use 'hh:mm tt' (e.g. 06:30 PM).");
                }
                var entity = new StaffAvailability
                {
                    StaffId = staffId,
                    StaffType = staffType,
                    Date = dto.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    AvailableSlots = dto.AvailableSlots,
                    SlotDuration = dto.SlotDuration,
                    Mode = dto.Mode,
                    Session = dto.Session,
                    SlotList = dto.SlotList,
                    ModifiedBy=id
                    

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
            try
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
            catch (Exception ex)
            {
                 return new ApiResponse<IEnumerable<StaffAvailabilityDTO>>(500, ex.Message);

            }
        
        }
    }
}