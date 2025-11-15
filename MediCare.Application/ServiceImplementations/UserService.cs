using MediCare.Application.Common;
using MediCare.Application.Contracts;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs;
using MediCare.Application.DTOs.AuthDTO;
using MediCare.Application.DTOs.UsersDTO;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Users> _userRepo;
        private readonly IGenericRepository<Patients> _patientRepo;
        private readonly IGenericRepository<Doctors> _doctorRepo;
        private readonly IGenericRepository<LabTechnicians> _labTechniciansRepo;
        private readonly IConfiguration _config;

        public UserService(IGenericRepository<Users> userRepo, IGenericRepository<Patients> patientRepo, IGenericRepository<Doctors> doctorRepo, IGenericRepository<LabTechnicians> labTechniciansRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _labTechniciansRepo = labTechniciansRepo;
            _config = config;
        }

        public async Task<AuthResponse> RegisterPatientAsync(RegisterRequestDTO request)
        {
            if (request == null) throw new ArgumentNullException("User data  cannot be null.");
            request.UserName = request.UserName.Trim();
            request.Password = request.Password.Trim();
            request.UserEmail = request.UserEmail.Trim().ToLower();

            var users = await _userRepo.GetAllAsync("Users");
            if (users.Any(u => u.UserEmail == request.UserEmail))
                return new AuthResponse(409, "Email already exists");


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Users
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                HashPassword = hashedPassword,
                Role = Roles.Patient,
            };

            await _userRepo.AddAsync("Users", newUser);

          

            return new AuthResponse(201, "User registered successfully");
        }

       

        public async Task<AuthResponse> RegisterDoctorAsync(RegisterRequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "User data cannot be null.");

            request.UserName = request.UserName.Trim();
            request.Password = request.Password.Trim();
            request.UserEmail = request.UserEmail.Trim().ToLower();

            var users = await _userRepo.GetAllAsync("Users");
            if (users.Any(u => u.UserEmail == request.UserEmail))
                return new AuthResponse(409, "Email already exists");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Users
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                HashPassword = hashedPassword,
                Role = Roles.Doctor,
            };

            await _userRepo.AddAsync("Users", newUser);

            //var insertedUser = (await _userRepo.GetAllAsync("Users"))
            //                    .First(u => u.UserEmail == request.UserEmail);

            //var newDoctor = new Doctors
            //{
            //    UserId = insertedUser.UserId,
            //    DepartmentId = null,
            //    ContactNumber = null,
            //    Fees = null,
            //    Experience = null,
            //    ProfilePhoto = null,
            //    MedicalRegistrationNumber = null,
            //    VerificationStatus = Veri_Status.Pending,
            //    IsAvailable=false
            //};

            //await _doctorRepo.AddAsync("SP_DOCTORS", newDoctor);

            return new AuthResponse(201, "Doctor registered successfully");
        }

        public async Task<AuthResponse> RegisterLabTechnicianAsync(RegisterRequestDTO request)
        {

            if (request == null) throw new ArgumentNullException("User data  cannot be null.");
            request.UserName = request.UserName.Trim();
            request.Password = request.Password.Trim();
            request.UserEmail = request.UserEmail.Trim().ToLower();

            var users = await _userRepo.GetAllAsync("Users");
            if (users.Any(u => u.UserEmail == request.UserEmail))
                return new AuthResponse(409, "Email already exists");


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Users
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                HashPassword = hashedPassword,
                Role = Roles.Labtechnician,
            };

            await _userRepo.AddAsync("Users", newUser);


            //var insertedUser = (await _userRepo.GetAllAsync("Users"))
            //                    .First(u => u.UserEmail == request.UserEmail);


            //var newLabTech = new LabTechnicians
            //{
            //    UserId = insertedUser.UserId,


            //};

            //await _labTechniciansRepo.AddAsync("SP_LABTECH", newLabTech);

            return new AuthResponse(201, "LabTechnician registered successfully");
        }

        public async Task<AuthResponse> LoginAsync(LoginRequestDTO request)
        {
            if (request == null) throw new ArgumentNullException("User data  cannot be null.");
            request.UserEmail = request.UserEmail.Trim().ToLower();
            request.Password = request.Password.Trim();

            var users = (await _userRepo.GetAllAsync("Users"))
                              .SingleOrDefault(u=>u.UserEmail.ToLower()==request.UserEmail.ToLower());
            if (users == null) return new AuthResponse(404, "User Doesnot Exist.");
            if (users.IsDeleted == true) return new AuthResponse(403, "Your Account is blocked. Please Contact Support.");
            

            if (!BCrypt.Net.BCrypt.Verify(request.Password, users.HashPassword))
                return new AuthResponse(401, "Invalid Password");

            var token = GenerateJwtToken(users);

            return new AuthResponse(200, "Login Successful,  Welcome to MediCare...",token);







        }

        private string GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                new Claim("userId",user.UserId.ToString()),
                new Claim("userName",user.UserName),
                new Claim("userRole",user.Role.ToString())

             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ApiResponse<List<DoctorInformationDTO>>> GetallDoctors()
        {
            try
            {
                var userdoctors = await _userRepo.GetAllAsync("Users");
                var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");

                var result = (from u in userdoctors
                              join d in doctors on u.UserId equals d.UserId
                              select new DoctorInformationDTO
                              {
                                  UserId = u.UserId,
                                  UserEmail=u.UserEmail,
                                  UserName = u.UserName,

                                  DoctorId=d.DoctorId,
                                  DepartmentId=d.DepartmentId,
                                  ProfilePhoto=d.ProfilePhoto,
                                  ContactNumber=d.ContactNumber,
                                  Experience=d.Experience,
                                  Fees=d.Fees,
                                  MedicalRegistrationNumber=d.MedicalRegistrationNumber,
                                  IsAvailable=d.IsAvailable,
                                  VerificationStatus=d.VerificationStatus
                              }).ToList();
                return new ApiResponse<List<DoctorInformationDTO>>(200, "Doctor details", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<DoctorInformationDTO>>(500, ex.Message);
            }
        }
        public async Task<ApiResponse<List<PatientInformationDTO>>> GetAllPatients()
        {
            try
            {
                var userPatients = await _userRepo.GetAllAsync("Users");
                var patients = await _patientRepo.GetAllAsync("SP_Patients");
                var result = (from u in userPatients
                              join p in patients on u.UserId equals p.UserId
                              select new PatientInformationDTO
                              {
                                  UserId = u.UserId,
                                  UserEmail = u.UserName,
                                  UserName = u.UserName,

                                  PatientId = p.PatientId,
                                  UHID = p.UHID,
                                  ContactNumber = p.ContactNumber,
                                  Age = p.Age,
                                  Gender = p.Gender

                              }).ToList();
                return new ApiResponse<List<PatientInformationDTO>>(200, "Patient Informations", result);

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PatientInformationDTO>>(500,ex.Message);
            }
        }
        public async Task<ApiResponse<List<LabtechnicianInformationDTO>>> getalllabtechnician()
        {
            try
            {
                var userLabtechnician = await _userRepo.GetAllAsync("Users");
                var labtechnicians = await _labTechniciansRepo.GetAllAsync("SP_LABTECH");
                var result= (from u in userLabtechnician
                             join l in labtechnicians on u.UserId equals l.UserId
                             select new LabtechnicianInformationDTO
                             {
                                 UserEmail = u.UserName,
                                 UserId=u.UserId,
                                 UserName=u.UserName,

                                 LabTechnicianId=l.LabTechnicianId,
                                 DepartmentId=l.DepartmentId,
                                 ContactNumber=l.ContactNumber,
                                 ProfilePhoto=l.ProfilePhoto,
                                 LicenceNumber=l.LicenceNumber,
                                 IsActive=l.IsActive,
                                 VerificationStatus=l.VerificationStatus
                             }).ToList();
                return new ApiResponse<List<LabtechnicianInformationDTO>>(200, "Labtechnicians Informations.", result);

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<LabtechnicianInformationDTO>>(500,ex.Message);
            }
        }
    }
    
}

