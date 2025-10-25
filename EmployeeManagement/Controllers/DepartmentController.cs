using EmployeeManagement.Models;
using EmployeeManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _departmentService.CreateDepartmentAsync(department);
                return CreatedAtAction(nameof(GetDepartmentById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input while creating department.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Duplicate department creation attempt.");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentService.GetDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department list.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                if (department == null)
                    return NotFound($"Department with ID {id} not found.");

                return Ok(department);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid ID provided.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching department by ID.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(string id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _departmentService.UpdateDepartmentAsync(id, department);
                if (updated == null)
                    return NotFound($"Department with ID {id} not found.");

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid update request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (!deleted)
                    return NotFound($"Department with ID {id} not found.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid delete request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
