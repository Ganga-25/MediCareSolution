using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestBooking;

using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class LabTestBookingService : ILabtestBookingService
    {
        private readonly IGenericRepository<PatientLabTest> _patientLabTestRepo;
        private readonly IGenericRepository<StaffAvailability> _staffAvailabilityRepo;
        private readonly IGenericRepository<LabTestBooking> _labTestBookingRepo;
        private readonly IGenericRepository<LabTests> _labtestRepo;
        private readonly IGenericRepository<LabTechnicians> _labtechniciansRepo;
        private readonly IGenericRepository<Patients> _patientRepo;
        public LabTestBookingService(
            IGenericRepository<PatientLabTest> patientLabTestRepo,
            IGenericRepository<StaffAvailability> staffAvailabilityRepo,
            IGenericRepository<LabTestBooking> labTestBookingRepo,
            IGenericRepository<LabTests> labtestRepo,
            IGenericRepository<LabTechnicians> labtechniciansRepo,
            IGenericRepository<Patients> patientRepo)
        {
            _patientLabTestRepo = patientLabTestRepo;
            _staffAvailabilityRepo = staffAvailabilityRepo;
            _labTestBookingRepo = labTestBookingRepo;
            _labtestRepo = labtestRepo;
            _labtechniciansRepo = labtechniciansRepo;
            _patientRepo = patientRepo;
        }

        public async Task<ApiResponse<string>> BookLabTestAsync(LabTestBookingDTO dto, int userId, string role)
        {
            try
            {
                // 1️⃣ Check if patient lab test exists
                var patientLabTest = await _patientLabTestRepo.GetByIdAsync("SP_PATIENTLABTEST", "ID", dto.Id);
                if (patientLabTest == null)
                    return new ApiResponse<string>(404, "Patient lab test not found");

                // 2️⃣ Check if lab test master exists
                var labTestMaster = await _labtestRepo.GetByIdAsync("SP_LABTEST", "LABTESTID", dto.LabTestId);
                if (labTestMaster == null)
                    return new ApiResponse<string>(404, "Lab test not found in master list");

                // 3️⃣ Ensure test is available in-house
                if (!patientLabTest.IsInHouse)
                    return new ApiResponse<string>(400, "Selected test is not available in the lab");

                // 4️⃣ Find available staff slot for LabTechnician
                var staffAvailabilities = (await _staffAvailabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY"))
                                            .Where(l => l.StaffType == StaffType.LabTechnician);

                var session = dto.LabTestTime.ToString(); // Morning / Afternoon
                var availableSlot = staffAvailabilities.FirstOrDefault(a =>
                    a.Date.Date == dto.LabTestDate.Date &&
                    (a.Session.Equals(session, StringComparison.OrdinalIgnoreCase) ||
                     a.Session.Equals("Fullday", StringComparison.OrdinalIgnoreCase)) &&
                    a.AvailableSlots > 0);

                if (availableSlot == null)
                    return new ApiResponse<string>(400, "No available slots for selected date or time");

                // 5️⃣ Get LabTechnician entity corresponding to the slot
                var labTechnician = (await _labtechniciansRepo.GetAllAsync("SP_LABTECH"))
                                    .FirstOrDefault(l => l.UserId == availableSlot.StaffId);

                if (labTechnician == null)
                    return new ApiResponse<string>(404, "LabTechnician not found");

                // 6️⃣ Check for duplicate booking
                var allBookings = await _labTestBookingRepo.GetAllAsync("SP_LABTESTBOOKING");
                var duplicate = allBookings.FirstOrDefault(b =>
                    b.PatientId == patientLabTest.PatientId &&
                    b.LabTestId == labTestMaster.LabTestId &&
                    b.Status == LabTestStatus.Scheduled);

                if (duplicate != null)
                    return new ApiResponse<string>(400, "You already have a booking for this test that is not yet completed");

                // 7️⃣ Create new booking with correct LabTechnicianId
                var newBooking = new LabTestBooking
                {
                    PatientId = patientLabTest.PatientId,
                    DoctorId = patientLabTest.DoctorId,
                    DepartmentId = patientLabTest.DepartmentId,
                    LabTestId = labTestMaster.LabTestId,
                    LabTechnicianId = labTechnician.LabTechnicianId, // ✅ Correct FK
                    Amount = labTestMaster.Price,
                    LabTestDate = dto.LabTestDate,
                    LabTestTime = dto.LabTestTime,
                    Status = LabTestStatus.Scheduled,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedBy = role,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false

                };

                var inserted = await _labTestBookingRepo.AddAsync("SP_LABTESTBOOKING", newBooking);
                if (inserted <= 0)
                    return new ApiResponse<string>(400, "Failed to book lab test");

                // 8️⃣ Reduce available slots
                availableSlot.AvailableSlots -= 1;
                await _staffAvailabilityRepo.UpdateAsync("SP_STAFF_AVAILABILITY", availableSlot);

                return new ApiResponse<string>(200, "Lab test booked successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }




        public async Task<ApiResponse<string>> CancelBookingAsync(int bookingId, int userId)
        {
            try
            {
                var booking = await _labTestBookingRepo.GetByIdAsync("SP_LABTESTBOOKING", "ID", bookingId);
                if (booking == null)
                    return new ApiResponse<string>(404, "Booking not found");

                if (booking.Status == LabTestStatus.Cancelled)
                    return new ApiResponse<string>(400, "Cannot cancel a completed booking");

                booking.Status = LabTestStatus.Cancelled;
                booking.PaymentStatus = PaymentStatus.Failed;
                booking.ModifiedBy = userId;


                await _labTestBookingRepo.UpdateAsync("SP_LABTESTBOOKING", booking);

                //  Reopen slot
                var staffAvailabilityList = await _staffAvailabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
                var slot = staffAvailabilityList.FirstOrDefault(a =>
                    a.StaffId == booking.LabTechnicianId &&
                    a.Date == booking.LabTestDate &&
                    a.Session == booking.LabTestTime.ToString());

                if (slot != null)
                {
                    slot.AvailableSlots += 1;
                    await _staffAvailabilityRepo.UpdateAsync("SP_STAFF_AVAILABILITY", slot);
                }

                return new ApiResponse<string>(200, "Booking cancelled successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<BookedLabTestViewDTO>>> GetbookedlabtestbyUser(int userId)
        {
            try
            {
                var patient= (await _patientRepo.GetAllAsync("SP_Patients")).SingleOrDefault(p=>p.UserId==userId);
                if (patient == null) return new ApiResponse<IEnumerable<BookedLabTestViewDTO>>(400, "No patient found");

                var bookedlabtest = (await _labTestBookingRepo.GetAllAsync("SP_LABTESTBOOKING"))
                                 .Where(b => b.PatientId == patient.PatientId);
                if (bookedlabtest == null) return new ApiResponse<IEnumerable<BookedLabTestViewDTO>>(404, "No labtest bookings for the user");

                var labtests = bookedlabtest.Select(s => new BookedLabTestViewDTO
                {
                    Id = s.Id,
                    LabTestDate = s.LabTestDate,
                    LabTestId = s.LabTestId,
                    LabTestTime = s.LabTestTime,
                    LabTechnicianId = s.LabTechnicianId,
                    DepartmentId = s.DepartmentId,
                    DoctorId = s.DoctorId,
                    Amount = s.Amount,
                    PatientId= s.PatientId,
                    PaymentMethod= s.PaymentMethod,
                    PaymentStatus= s.PaymentStatus,
                    Status= s.Status
                    

                }).ToList();

                return new ApiResponse<IEnumerable<BookedLabTestViewDTO>>(200, "Booked Labtests fetched successfully.", labtests);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<BookedLabTestViewDTO>>(500, ex.Message);
            }
        }
    }
}