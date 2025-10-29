using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AppointmentDTO;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<StaffAvailability> _availabilityRepo;
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<Doctors> _doctorRepo;
        private readonly IGenericRepository<Patients> _patientRepo;

        public AppointmentService(
            IGenericRepository<StaffAvailability> availabilityRepo,
            IGenericRepository<Appointment> appointmentRepo,
            IGenericRepository<Doctors> doctorRepo,
            IGenericRepository<Patients> patientRepo)
        {
            _availabilityRepo = availabilityRepo;
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
        }

        public async Task<ApiResponse<string>> BookAppointmentAsync(AppointmentDTO dto)
        {
            // 🔹 Step 1: Validate doctor
            var doctor = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "@DOCTORID", dto.DoctorId);
            if (doctor == null)
                return new ApiResponse<string>(404, "Doctor not found.");

            // 🔹 Step 2: Get doctor availability (StaffId == Doctor.UserId)
            var allAvailabilities = await _availabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
            var doctorAvailabilities = allAvailabilities
                .Where(a => a.StaffId == doctor.UserId && a.Date.Date == dto.AppointmentDate.Date)
                .ToList();

            if (doctorAvailabilities.Count == 0)
                return new ApiResponse<string>(400, "No availability found for the selected doctor/date.");

            // 🔹 Step 3: Find specific availability that contains the requested slot
            var doctorAvailability = doctorAvailabilities.FirstOrDefault(a =>
                !string.IsNullOrEmpty(a.SlotList) &&
                a.SlotList.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .Contains(dto.AppointmentTime.ToString(), StringComparer.OrdinalIgnoreCase));

            if (doctorAvailability == null)
                return new ApiResponse<string>(400, "Invalid slot. Doctor not available at that time.");

            // 🔹 Step 4: Check if slot is already booked
            var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");

            // Convert DTO time string to TimeSpan to match DB column
            TimeSpan parsedTime;
            try
            {
                parsedTime = DateTime.ParseExact(dto.AppointmentTime.ToString(), "hh:mm tt", null).TimeOfDay;
            }
            catch
            {
                return new ApiResponse<string>(400, "Invalid appointment time format. Use 'hh:mm tt' (e.g. 06:30 PM).");
            }

            bool isAlreadyBooked = allAppointments.Any(a =>
                a.DoctorId == doctor.DoctorId &&
                a.AppointmentDate.Date == dto.AppointmentDate.Date &&
                a.AppointmentTime == parsedTime);

            if (isAlreadyBooked)
                return new ApiResponse<string>(400, "Slot already booked by another patient.");

            // 🔹 Step 5: Create and save appointment
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                UHID = dto.UHID,
                DoctorId = doctor.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = parsedTime,  // ✅ Stored as TimeSpan
                Mode = dto.Mode,
                Notes = dto.Notes
            };

            await _appointmentRepo.AddAsync("SP_APPOINTMENT", appointment);

            // 🔹 Step 6: Decrease available slots (if > 0)
            if (doctorAvailability.AvailableSlots > 0)
                doctorAvailability.AvailableSlots--;

            await _availabilityRepo.UpdateAsync("SP_STAFF_AVAILABILITY", doctorAvailability);

            return new ApiResponse<string>(200, "Appointment booked successfully.");
        }

        public async Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAllAppointmentsAsync()
        {
            var data = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");
            var appointments = data.Select(x => new AppointmentDetailsDTO
            {
                AppointmentId = x.AppointmentId,
                PatientId = x.PatientId,
                UHID = x.UHID,
                DoctorId = x.DoctorId,
                AppointmentDate = x.AppointmentDate,
                AppointmentTime = x.AppointmentTime.ToString(@"hh\:mm"),
                AppointmentStatus=x.AppointmentStatus,
                Mode = x.Mode,
                Notes = x.Notes
            }).ToList();

            return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>
            (200, "All Appointments Fetched Successfully", appointments);
        }


        public async Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAppointmentsByPatientIdAsync(int userId)
        {
            // Get PatientId using UserId
            var patients = await _patientRepo.GetAllAsync("SP_Patients");
            var patient = patients.FirstOrDefault(p => p.UserId == userId);

            if (patient == null)
                return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(404, "Patient not found for this user.");
                

            var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");
            var filtered = allAppointments.Where(a => a.PatientId == patient.PatientId && a.AppointmentStatus==AppointmentStatus.Scheduled && a.RescheduleStatus==ScheduleStatus.Rescheduled);

            var appointments= filtered.Select(x=>new AppointmentDetailsDTO
            {

                AppointmentId = x.AppointmentId,
                PatientId=x.PatientId,
                UHID = x.UHID,
                DoctorId = x.DoctorId,
                AppointmentDate=x.AppointmentDate,
                AppointmentTime= x.AppointmentTime.ToString(@"hh\:mm"),
                AppointmentStatus=x.AppointmentStatus,
                Mode = x.Mode,
                Notes = x.Notes
            }).ToList();

            return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(200, "Appointments fetched for the patient successfully.", appointments);
            }

        public async Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");
            var filtered = allAppointments.Where(a => a.DoctorId == doctorId);

            var appointments = filtered.Select(x => new AppointmentDetailsDTO
            {
                AppointmentId=x.AppointmentId,
                PatientId = x.PatientId,
                UHID = x.UHID,
                DoctorId = x.DoctorId,
                AppointmentDate = x.AppointmentDate,
                AppointmentTime = x.AppointmentTime.ToString(@"hh\:mm"),
                AppointmentStatus= x.AppointmentStatus,
                Mode = x.Mode,
                Notes = x.Notes
            }).ToList();

            return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(200, "Appointments fetched for the doctor successfully.", appointments);
            
        }
        public async Task<ApiResponse<string>> CancelAppointmentAsync(CancelAppointmentDTO dto, int userId, string role)
        {
            var appointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");
            Appointment? appointment = null;

            if (role == "Admin")
            {
                // Admin can cancel any appointment
                appointment = appointments.FirstOrDefault(a => a.AppointmentId == dto.AppointmentId);
            }
            else
            {
                // Patient can cancel only their own
                var patients = await _patientRepo.GetAllAsync("SP_Patients");
                var patient = patients.FirstOrDefault(p => p.UserId == userId);
                if (patient == null)
                    return new ApiResponse<string>(404, "Patient not found");

                appointment = appointments.FirstOrDefault(a => a.AppointmentId == dto.AppointmentId && a.PatientId == patient.PatientId);
            }

            if (appointment == null)
                return new ApiResponse<string>(404, "Appointment not found or not accessible");

            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            appointment.CancellationReason = dto.CancellationReason ?? "Cancelled";
            

            await _appointmentRepo.UpdateAsync("SP_APPOINTMENT", appointment);

            return new ApiResponse<string>(200, "Appointment cancelled successfully");
        }

        public async Task<ApiResponse<string>> RescheduleAppointmentAsync(RescheduleAppointmentDTO dto, int userId, string role)
{
    // Step 1: Get appointment details
    var appointment = await _appointmentRepo.GetByIdAsync("SP_APPOINTMENT", "@APPOINTMENTID", dto.AppointmentId);
    if (appointment == null)
        return new ApiResponse<string>(404, "Appointment not found");

    // Step 2: Validate ownership (patients can only reschedule their own)
    if (role == "Patient" && appointment.PatientId != userId)
        return new ApiResponse<string>(403, "You cannot modify someone else’s appointment");

    // Step 3: Save old values before changing
    appointment.OldDate = DateOnly.FromDateTime(appointment.AppointmentDate);
    appointment.OldTimeSlot = TimeOnly.FromTimeSpan(appointment.AppointmentTime);
    appointment.OldDoctorId = appointment.DoctorId;

    // Step 4: Apply new changes based on reschedule type
    switch (dto.RescheduleType)
    {
        case Reschedule_Type.TimeOnly:
            if (dto.NewDate == null || dto.NewTime == null)
                return new ApiResponse<string>(400, "New date and time are required");
            appointment.NewDate = DateOnly.FromDateTime(dto.NewDate.Value);
            appointment.NewTimeSlot = TimeOnly.FromTimeSpan(dto.NewTime.Value);
            appointment.AppointmentDate = dto.NewDate.Value;
            appointment.AppointmentTime = dto.NewTime.Value;
            break;

        case Reschedule_Type.DoctorOnly:
            if (dto.NewDoctorId == null)
                return new ApiResponse<string>(400, "New doctor ID is required");
            appointment.NewDoctorId = dto.NewDoctorId;
            appointment.DoctorId = dto.NewDoctorId.Value;
            break;

        case Reschedule_Type.Both:
            if (dto.NewDate == null || dto.NewTime == null || dto.NewDoctorId == null)
                return new ApiResponse<string>(400, "New date, time, and doctor are required");
            appointment.NewDate = DateOnly.FromDateTime(dto.NewDate.Value);
            appointment.NewTimeSlot = TimeOnly.FromTimeSpan(dto.NewTime.Value);
            appointment.NewDoctorId = dto.NewDoctorId;
            appointment.AppointmentDate = dto.NewDate.Value;
            appointment.AppointmentTime = dto.NewTime.Value;
            appointment.DoctorId = dto.NewDoctorId.Value;
            break;

        default:
            return new ApiResponse<string>(400, "Invalid reschedule type");
    }

    // Step 5: Update reschedule details
    appointment.RescheduleType = dto.RescheduleType;
    appointment.RescheduleReason = dto.RescheduleReason ?? "Doctor not available";
    appointment.RescheduleStatus = ScheduleStatus.Rescheduled;
    appointment.AppointmentStatus = AppointmentStatus.Cancelled;

    // Step 6: Update database (SP_APPOINTMENT handles @FLAG='UPDATE')
    var result = await _appointmentRepo.UpdateAsync("SP_APPOINTMENT", appointment);

    if (result > 0)
        return new ApiResponse<string>(200, "Appointment rescheduled successfully");
    else
        return new ApiResponse<string>(500, "Failed to reschedule appointment");
}

    }



    }

