using EmployeeManagement.Models;
using EmployeeManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _employeeService.CreateEmployeeAsync(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input while creating employee.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Duplicate employee creation attempt.");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee list.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound($"Employee with ID {id} not found.");

                return Ok(employee);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid ID provided.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee by ID.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _employeeService.UpdateEmployeeAsync(id, employee);
                if (updated == null)
                    return NotFound($"Employee with ID {id} not found.");

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid update request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (!deleted)
                    return NotFound($"Employee with ID {id} not found.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid delete request.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
