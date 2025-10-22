using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.DepartmentDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllDepartment()
        {
            var result = await _departmentService.GetAllDepartmentAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDTO adddto)
        {
            var result = await _departmentService.AddDepartmentAsync(adddto);
            return StatusCode(result.StatusCode, result);

        }
    }
}
