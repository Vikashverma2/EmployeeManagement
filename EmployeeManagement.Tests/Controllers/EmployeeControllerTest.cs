using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockService;
        private readonly Mock<ILogger<EmployeeController>> _mockLogger;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockService = new Mock<IEmployeeService>();
            _mockLogger = new Mock<ILogger<EmployeeController>>();
            _controller = new EmployeeController(_mockService.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task CreateEmployee_ReturnsCreated_WhenValid()
        {
            var employee = new Employee { Id = "1", FullName = "Vikash Verma" };
            _mockService.Setup(s => s.CreateEmployeeAsync(employee))
                        .ReturnsAsync(employee);

            var result = await _controller.CreateEmployee(employee);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var data = Assert.IsType<Employee>(created.Value);
            Assert.Equal("Vikash Verma", data.FullName);

            _mockService.Verify();
        }

        [Fact]
        public async Task CreateEmployee_ReturnsBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.CreateEmployee(new Employee());

            Assert.IsType<BadRequestObjectResult>(result);
            _mockService.Verify();
        }

        [Fact]
        public async Task CreateEmployee_ReturnsBadRequest_OnArgumentException()
        {
            _mockService.Setup(s => s.CreateEmployeeAsync(It.IsAny<Employee>()))
                        .ThrowsAsync(new ArgumentException("Invalid input"));

            var result = await _controller.CreateEmployee(new Employee());

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid input", bad.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task CreateEmployee_ReturnsConflict_OnInvalidOperationException()
        {
            _mockService.Setup(s => s.CreateEmployeeAsync(It.IsAny<Employee>()))
                        .ThrowsAsync(new InvalidOperationException("Duplicate"));

            var result = await _controller.CreateEmployee(new Employee());

            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Duplicate", conflict.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task CreateEmployee_Returns500_OnException()
        {
            _mockService.Setup(s => s.CreateEmployeeAsync(It.IsAny<Employee>()))
                        .ThrowsAsync(new Exception("Server error"));

            var result = await _controller.CreateEmployee(new Employee());

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);

            _mockService.Verify();
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsOk()
        {
            var list = new List<Employee> { new Employee { Id = "1", FullName = "Vikash Verma" } };
            _mockService.Setup(s => s.GetEmployeesAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllEmployees();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Employee>>(ok.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task GetAllEmployees_Returns500_OnException()
        {
            _mockService.Setup(s => s.GetEmployeesAsync())
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetAllEmployees();

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);

            _mockService.Verify();
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsOk_WhenExists()
        {
            var emp = new Employee { Id = "1", FullName = "Vikash Verma" };
            _mockService.Setup(s => s.GetEmployeeByIdAsync("1")).ReturnsAsync(emp);

            var result = await _controller.GetEmployeeById("1");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Employee>(ok.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.GetEmployeeByIdAsync("1")).ReturnsAsync((Employee)null);

            var result = await _controller.GetEmployeeById("1");

            Assert.IsType<NotFoundObjectResult>(result);

            _mockService.Verify();
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsBadRequest_OnArgumentException()
        {
            _mockService.Setup(s => s.GetEmployeeByIdAsync("1"))
                        .ThrowsAsync(new ArgumentException("Invalid id"));

            var result = await _controller.GetEmployeeById("1");

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid id", bad.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task GetEmployeeById_Returns500_OnException()
        {
            _mockService.Setup(s => s.GetEmployeeByIdAsync("1"))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetEmployeeById("1");

            Assert.Equal(500, (result as ObjectResult)?.StatusCode);

            _mockService.Verify();
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsOk_WhenUpdated()
        {
            var emp = new Employee { Id = "1", FullName = "Vikash Verma" };
            _mockService.Setup(s => s.UpdateEmployeeAsync("1", emp)).ReturnsAsync(emp);

            var result = await _controller.UpdateEmployee("1", emp);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Employee>(ok.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.UpdateEmployee("1", new Employee());

            Assert.IsType<BadRequestObjectResult>(result);

            _mockService.Verify();
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNotFound_WhenIdNotFound()
        {
            _mockService.Setup(s => s.UpdateEmployeeAsync("1", It.IsAny<Employee>()))
                        .ReturnsAsync((Employee)null);

            var result = await _controller.UpdateEmployee("1", new Employee());

            Assert.IsType<NotFoundObjectResult>(result);

            _mockService.Verify();
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsBadRequest_OnArgumentException()
        {
            _mockService.Setup(s => s.UpdateEmployeeAsync("1", It.IsAny<Employee>()))
                        .ThrowsAsync(new ArgumentException("Invalid update"));

            var result = await _controller.UpdateEmployee("1", new Employee());

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid update", bad.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task UpdateEmployee_Returns500_OnException()
        {
            _mockService.Setup(s => s.UpdateEmployeeAsync("1", It.IsAny<Employee>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.UpdateEmployee("1", new Employee());

            Assert.Equal(500, (result as ObjectResult)?.StatusCode);

            _mockService.Verify();
        }


        [Fact]
        public async Task DeleteEmployee_ReturnsNoContent_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteEmployeeAsync("1")).ReturnsAsync(true);

            var result = await _controller.DeleteEmployee("1");

            Assert.IsType<NoContentResult>(result);

            _mockService.Verify();
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.DeleteEmployeeAsync("1")).ReturnsAsync(false);

            var result = await _controller.DeleteEmployee("1");

            Assert.IsType<NotFoundObjectResult>(result);

            _mockService.Verify();
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsBadRequest_OnArgumentException()
        {
            _mockService.Setup(s => s.DeleteEmployeeAsync("1"))
                        .ThrowsAsync(new ArgumentException("Invalid delete"));

            var result = await _controller.DeleteEmployee("1");

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid delete", bad.Value);

            _mockService.Verify();
        }

        [Fact]
        public async Task DeleteEmployee_Returns500_OnException()
        {
            _mockService.Setup(s => s.DeleteEmployeeAsync("1"))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.DeleteEmployee("1");

            Assert.Equal(500, (result as ObjectResult)?.StatusCode);

            _mockService.Verify();
        }
    }
}
