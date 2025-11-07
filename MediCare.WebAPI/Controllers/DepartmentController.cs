using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.DepartmentDTO;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("Departments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDepartment()
        {
            string? role = null;

     
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                role = User.GetUserRole(); 
            }
            var result = await _departmentService.GetAllDepartmentAsync(role);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result= await _departmentService.GetDepartmentById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromForm] AddDepartmentDTO adddto)
        {
            var result = await _departmentService.AddDepartmentAsync(adddto);
            return StatusCode(result.StatusCode, result);

        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm]UpdateDepartmentDTO dTO)
        {
            int userId=User.GetUserId();
            var result= await _departmentService.UpdateDepartment(dTO,userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm]int departmentId)
        {
          int userId=User.GetUserId() ;
            var result= await _departmentService.DeleteDepartmentAsync(departmentId,userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch]
        public async Task<IActionResult> Reactivate([FromForm]int departmentId)
        {
            int userId = User.GetUserId();
            var result= await _departmentService.ActivateandDeactivateDepartment(departmentId,userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
