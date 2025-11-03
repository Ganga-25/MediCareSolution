using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AppointmentDTO;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<ApiResponse<string>> BookAppointmentAsync(AppointmentDTO dto, int userId)
        {
            // 🔹 Step 1: Validate doctor
            var patients = await _patientRepo.GetAllAsync("SP_Patients");
            var patient = patients.SingleOrDefault(p => p.UserId == userId);
            if (patient == null) return new ApiResponse<string>(404, "Patient not found");


            var doctor = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "@DOCTORID", dto.DoctorId);
            if (doctor == null)
                return new ApiResponse<string>(404, "Doctor not found.");


            var allAvailabilities = await _availabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
            var doctorAvailabilities = allAvailabilities
                .Where(a => a.StaffId == doctor.DoctorId && a.Date.Date == dto.AppointmentDate.Date)
                .ToList();

            if (doctorAvailabilities.Count == 0)
                return new ApiResponse<string>(400, "No availability found for the selected doctor/date.");


            var doctorAvailability = doctorAvailabilities.FirstOrDefault(a =>
                !string.IsNullOrEmpty(a.SlotList) &&
                a.SlotList.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .Contains(dto.AppointmentTime.ToString(), StringComparer.OrdinalIgnoreCase));

            if (doctorAvailability == null)
                return new ApiResponse<string>(400, "Invalid slot. Doctor not available at that time.");


            var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");


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
                a.AppointmentTime == parsedTime &&
                a.AppointmentStatus==AppointmentStatus.Scheduled);

            if (isAlreadyBooked)
                return new ApiResponse<string>(400, "Slot already booked by another patient.");


            var appointment = new Appointment
            {
                PatientId = patient.PatientId,
                UHID = patient.UHID,
                DoctorId = doctor.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = parsedTime,
                Mode = dto.Mode,
                Notes = dto.Notes
            };

            await _appointmentRepo.AddAsync("SP_APPOINTMENT", appointment);


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
                AppointmentStatus = x.AppointmentStatus,
                Mode = x.Mode,
                Notes = x.Notes
            }).ToList();

            return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>
            (200, "All Appointments Fetched Successfully", appointments);
        }

        public async Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAppointmentsByPatientIdAsync(int userId)
        {
            try
            {
                // Get PatientId using UserId
                var patients = await _patientRepo.GetAllAsync("SP_Patients");
                var patient = patients.SingleOrDefault(p => p.UserId == userId);

                if (patient == null)
                    return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(404, "Patient not found.");

                var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");
                var patientAppointments = allAppointments.Where(p => p.PatientId == patient.PatientId).ToList();

                if (!patientAppointments.Any())
                    return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(404, "No Appointments for this patient.");

                var appointments = patientAppointments.Select(a =>
                {
                    bool isRescheduled = a.RescheduleStatus ==ScheduleStatus.Rescheduled;

                    return new AppointmentDetailsDTO
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        UHID = a.UHID,
                        DoctorId = isRescheduled ? a.NewDoctorId ?? a.DoctorId : a.DoctorId,
                        AppointmentDate = isRescheduled ? a.NewDate ?? a.AppointmentDate : a.AppointmentDate,
                        AppointmentTime = isRescheduled
                            ? (a.NewTimeSlot != null ? a.NewTimeSlot.Value.ToString(@"hh\:mm") : a.AppointmentTime.ToString(@"hh\:mm"))
                            : a.AppointmentTime.ToString(@"hh\:mm"),
                        Mode = a.Mode,
                        AppointmentStatus = a.AppointmentStatus,
                        Notes = a.Notes,
                        RescheduleType = a.RescheduleType,
                        RescheduleStatus = a.RescheduleStatus,
                        RescheduleReason = a.RescheduleReason,
                        NewDate = a.NewDate,
                        NewTimeSlot = a.NewTimeSlot?.ToString(@"hh\:mm"),
                        NewDoctorId = a.NewDoctorId,

                    };
                }).ToList();

                return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(200, "Appointments fetched successfully.", appointments);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AppointmentDetailsDTO>>(500, ex.Message);
            }
        }


        public async Task<ApiResponse<IEnumerable<AppointmentDetailsDTO>>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENT");
            var filtered = allAppointments.Where(a => a.DoctorId == doctorId);

            var appointments = filtered.Select(a =>
            {
                bool isRescheduled = a.RescheduleStatus == ScheduleStatus.Rescheduled;

                return new AppointmentDetailsDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    UHID = a.UHID,
                    DoctorId = isRescheduled ? a.NewDoctorId ?? a.DoctorId : a.DoctorId,
                    AppointmentDate = isRescheduled ? a.NewDate ?? a.AppointmentDate : a.AppointmentDate,
                    AppointmentTime = isRescheduled
                        ? (a.NewTimeSlot != null ? a.NewTimeSlot.Value.ToString(@"hh\:mm") : a.AppointmentTime.ToString(@"hh\:mm"))
                        : a.AppointmentTime.ToString(@"hh\:mm"),
                    Mode = a.Mode,
                    AppointmentStatus = a.AppointmentStatus,
                    Notes = a.Notes,
                    RescheduleType = a.RescheduleType,
                    RescheduleStatus = a.RescheduleStatus,
                    RescheduleReason = a.RescheduleReason,
                    NewDate = a.NewDate,
                    NewTimeSlot = a.NewTimeSlot?.ToString(@"hh\:mm"),
                    NewDoctorId = a.NewDoctorId,

                };
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
            appointment.ModifiedBy = userId;


            await _appointmentRepo.UpdateAsync("SP_APPOINTMENT", appointment);

            return new ApiResponse<string>(200, "Appointment cancelled successfully");
        }

        public async Task<ApiResponse<string>> RescheduleAppointmentAsync(RescheduleAppointmentDTO dto, int userId, string role)
        {

            try
            {
                var appointment = await _appointmentRepo.GetByIdAsync("SP_APPOINTMENT", "APPOINTMENTID", dto.AppointmentId);
                if (appointment == null)
                    return new ApiResponse<string>(404, "Appointment not found");

                if (role == "Patient")
                {
                    var allPatients = await _patientRepo.GetAllAsync("SP_Patients");
                    var patient = allPatients.SingleOrDefault(p => p.UserId == userId);

                    if (patient == null)
                        return new ApiResponse<string>(404, "Patient record not found");


                    if (appointment.PatientId != patient.PatientId)
                        return new ApiResponse<string>(403, "You cannot modify someone else’s appointment");
                }

                TimeSpan parsedTime;
                try
                {
                    parsedTime = DateTime.ParseExact(dto.NewTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                }
                catch (FormatException)
                {
                    return new ApiResponse<string>(400, "Invalid appointment time format. Use 'hh:mm tt' (e.g. 06:30 PM).");
                }


                appointment.OldDate = appointment.AppointmentDate;
                appointment.OldTimeSlot = appointment.AppointmentTime;
                appointment.OldDoctorId = appointment.DoctorId;

                switch (dto.RescheduleType)
                {
                    case Reschedule_Type.TimeOnly:
                        if (dto.NewDate == null || dto.NewTime == null)
                            return new ApiResponse<string>(400, "New date and time are required");

                        var doctor = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "DOCTORID", appointment.DoctorId);
                        if (doctor == null)
                            return new ApiResponse<string>(404, "Doctor not found.");

                        var allAvailabilities = await _availabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
                        var doctorAvailabilities = allAvailabilities
                            .Where(a => a.StaffId == doctor.DoctorId && a.Date.Date == dto.NewDate.Date)
                            .ToList();

                        if (doctorAvailabilities.Count == 0)
                            return new ApiResponse<string>(400, "No availability found for the selected doctor/date.");

                        var doctorAvailability = doctorAvailabilities.FirstOrDefault(a =>
                            !string.IsNullOrEmpty(a.SlotList) &&
                            a.SlotList.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim())
                                      .Contains(dto.NewTime.ToString(), StringComparer.OrdinalIgnoreCase));

                        if (doctorAvailability == null)
                            return new ApiResponse<string>(400, "Selected time slot is not available for this doctor.");


                        appointment.NewDate = dto.NewDate;
                        appointment.NewTimeSlot = parsedTime;
                        appointment.AppointmentDate = dto.NewDate;
                        appointment.AppointmentTime = parsedTime;
                        appointment.NewDoctorId= appointment.DoctorId;
                        break;


                    case Reschedule_Type.DoctorOnly:

                        if (dto.NewDoctorId == null)
                            return new ApiResponse<string>(400, "New doctor ID is required");


                        var newDoctor = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "DOCTORID", dto.NewDoctorId);
                        if (newDoctor == null)
                            return new ApiResponse<string>(404, "New doctor not found.");


                        var allAvailabilitie = await _availabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
                        var doctorAvailabilitie = allAvailabilitie
                            .Where(a => a.StaffId == newDoctor.DoctorId && a.Date.Date == appointment.AppointmentDate.Date)
                            .ToList();

                        if (doctorAvailabilitie.Count == 0)
                            return new ApiResponse<string>(400, "Selected doctor has no availability on the current appointment date.");


                        var doctorAvailabilit = doctorAvailabilitie.FirstOrDefault(a =>
                            !string.IsNullOrEmpty(a.SlotList) &&
                            a.SlotList.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim())
                                      .Contains(appointment.AppointmentTime.ToString(@"hh\\:mm"), StringComparer.OrdinalIgnoreCase));

                        if (doctorAvailabilit == null)
                            return new ApiResponse<string>(400, "Selected doctor is not available at this appointment time.");


                        appointment.NewDoctorId = dto.NewDoctorId;
                        appointment.AppointmentDate = appointment.AppointmentDate;
                        appointment.AppointmentTime = appointment.AppointmentTime;
                       
                        break;



                    case Reschedule_Type.Both:
                        {
                            if (dto.NewDate == null || dto.NewTime == null || dto.NewDoctorId == null)
                                return new ApiResponse<string>(400, "New date, time, and doctor are required");


                            var newDoctors = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "DOCTORID", dto.NewDoctorId);
                            if (newDoctors == null)
                                return new ApiResponse<string>(404, "doctor not found.");


                            var allAvailability = await _availabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
                            var doctorAvailabili = allAvailability
                                .Where(a => a.StaffId == newDoctors.DoctorId && a.Date.Date == dto.NewDate.Date)
                                .ToList();

                            if (doctorAvailabili.Count == 0)
                                return new ApiResponse<string>(400, "Selected doctor has no availability on the chosen date.");


                            var doctorAvailable = doctorAvailabili.FirstOrDefault(a =>
                                !string.IsNullOrEmpty(a.SlotList) &&
                                a.SlotList.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(s => s.Trim())
                                          .Contains(dto.NewTime.ToString(), StringComparer.OrdinalIgnoreCase));

                            if (doctorAvailable == null)
                                return new ApiResponse<string>(400, "Selected doctor is not available at the requested time on this date.");


                            appointment.NewDate = dto.NewDate;
                            appointment.NewTimeSlot = parsedTime;
                            appointment.NewDoctorId = dto.NewDoctorId;
                            appointment.AppointmentDate = dto.NewDate;
                            appointment.AppointmentTime = parsedTime;
                            appointment.DoctorId = dto.NewDoctorId.Value;
                            break;
                        }


                    default:
                        return new ApiResponse<string>(400, "Invalid reschedule type");
                }


                appointment.RescheduleType = dto.RescheduleType;
                appointment.RescheduleReason = dto.RescheduleReason ?? "Doctor not available";
                appointment.RescheduleStatus = ScheduleStatus.Rescheduled;
                appointment.AppointmentStatus = AppointmentStatus.Cancelled;

                var result = await _appointmentRepo.UpdateAsync("SP_APPOINTMENT", appointment);

                if (result > 0)
                    return new ApiResponse<string>(200, "Appointment rescheduled successfully");
                else
                    return new ApiResponse<string>(500, "Failed to reschedule appointment");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }




        }

    }
}