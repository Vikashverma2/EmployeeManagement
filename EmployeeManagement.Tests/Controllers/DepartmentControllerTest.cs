using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Tests.Controllers
{
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> _mockService;
        private readonly Mock<ILogger<DepartmentController>> _mockLogger;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _mockService = new Mock<IDepartmentService>();
            _mockLogger = new Mock<ILogger<DepartmentController>>();
            _controller = new DepartmentController(_mockService.Object, _mockLogger.Object);
        }


        //  CREATE DEPARTMENT (POST)

        [Fact]
        public async Task CreateDepartment_ValidInput_ReturnsCreated()
        {
            // Arrange
            var input = new Department { Name = "HR" };
            var created = new Department { Id = "1", Name = "HR" };
            _mockService.Setup(s => s.CreateDepartmentAsync(It.IsAny<Department>()))
                        .ReturnsAsync(created);

            // Act
            var result = await _controller.CreateDepartment(input);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var dept = Assert.IsType<Department>(createdResult.Value);
            Assert.Equal("HR", dept.Name);

            // Verify that the service was called once
            _mockService.Verify(s => s.CreateDepartmentAsync(It.IsAny<Department>()), Times.Once);
        }

        [Fact]
        public async Task CreateDepartment_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var input = new Department();

            // Act
            var result = await _controller.CreateDepartment(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _mockService.Verify(s => s.CreateDepartmentAsync(It.IsAny<Department>()), Times.Never);
        }

        [Fact]
        public async Task CreateDepartment_ArgumentException_ReturnsBadRequest()
        {
            var input = new Department { Name = "Finance" };
            _mockService.Setup(s => s.CreateDepartmentAsync(It.IsAny<Department>()))
                        .ThrowsAsync(new ArgumentException("Invalid input"));

            var result = await _controller.CreateDepartment(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid input", badRequest.Value);
        }

        [Fact]
        public async Task CreateDepartment_Duplicate_ReturnsConflict()
        {
            var input = new Department { Name = "IT" };
            _mockService.Setup(s => s.CreateDepartmentAsync(It.IsAny<Department>()))
                        .ThrowsAsync(new InvalidOperationException("Duplicate department"));

            var result = await _controller.CreateDepartment(input);

            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Duplicate department", conflict.Value);
        }

        [Fact]
        public async Task CreateDepartment_Exception_Returns500()
        {
            var input = new Department { Name = "Admin" };
            _mockService.Setup(s => s.CreateDepartmentAsync(It.IsAny<Department>()))
                        .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.CreateDepartment(input);

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }

        // GET ALL DEPARTMENTS (GET)

        [Fact]
        public async Task GetAllDepartments_ReturnsOk_WithData()
        {
            var departments = new List<Department>
            {
                new Department { Id = "1", Name = "HR" },
                new Department { Id = "2", Name = "IT" }
            };

            _mockService.Setup(s => s.GetDepartmentsAsync()).ReturnsAsync(departments);

            var result = await _controller.GetAllDepartments();

            var ok = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<List<Department>>(ok.Value);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllDepartments_WhenException_Returns500()
        {
            _mockService.Setup(s => s.GetDepartmentsAsync()).ThrowsAsync(new Exception("DB down"));

            var result = await _controller.GetAllDepartments();

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }


        // GET DEPARTMENT BY ID (GET /id)

        [Fact]
        public async Task GetDepartmentById_Found_ReturnsOk()
        {
            var department = new Department { Id = "1", Name = "HR" };
            _mockService.Setup(s => s.GetDepartmentByIdAsync("1")).ReturnsAsync(department);

            var result = await _controller.GetDepartmentById("1");

            var ok = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<Department>(ok.Value);
            Assert.Equal("HR", data.Name);
        }

        [Fact]
        public async Task GetDepartmentById_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetDepartmentByIdAsync("1")).ReturnsAsync((Department)null);

            var result = await _controller.GetDepartmentById("1");

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("not found", notFound.Value.ToString());
        }

        [Fact]
        public async Task GetDepartmentById_InvalidId_ReturnsBadRequest()
        {
            _mockService.Setup(s => s.GetDepartmentByIdAsync("invalid"))
                        .ThrowsAsync(new ArgumentException("Invalid ID"));

            var result = await _controller.GetDepartmentById("invalid");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetDepartmentById_Exception_Returns500()
        {
            _mockService.Setup(s => s.GetDepartmentByIdAsync("1"))
                        .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.GetDepartmentById("1");

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }


        // UPDATE DEPARTMENT (PUT)

        [Fact]
        public async Task UpdateDepartment_Valid_ReturnsOk()
        {
            var input = new Department { Id = "1", Name = "Finance" };
            _mockService.Setup(s => s.UpdateDepartmentAsync("1", input)).ReturnsAsync(input);

            var result = await _controller.UpdateDepartment("1", input);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dept = Assert.IsType<Department>(ok.Value);
            Assert.Equal("Finance", dept.Name);
        }

        [Fact]
        public async Task UpdateDepartment_NotFound_ReturnsNotFound()
        {
            var input = new Department { Id = "1", Name = "Finance" };
            _mockService.Setup(s => s.UpdateDepartmentAsync("1", input)).ReturnsAsync((Department)null);

            var result = await _controller.UpdateDepartment("1", input);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("not found", notFound.Value.ToString());
        }

        [Fact]
        public async Task UpdateDepartment_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.UpdateDepartment("1", new Department());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateDepartment_ArgumentException_ReturnsBadRequest()
        {
            var input = new Department { Id = "1", Name = "Finance" };
            _mockService.Setup(s => s.UpdateDepartmentAsync("1", input))
                        .ThrowsAsync(new ArgumentException("Invalid update request."));

            var result = await _controller.UpdateDepartment("1", input);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateDepartment_Exception_Returns500()
        {
            var input = new Department { Id = "1", Name = "Finance" };
            _mockService.Setup(s => s.UpdateDepartmentAsync("1", input))
                        .ThrowsAsync(new Exception("Unexpected"));

            var result = await _controller.UpdateDepartment("1", input);

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }

        // DELETE DEPARTMENT (DELETE)

        [Fact]
        public async Task DeleteDepartment_Success_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteDepartmentAsync("1")).ReturnsAsync(true);

            var result = await _controller.DeleteDepartment("1");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDepartment_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.DeleteDepartmentAsync("1")).ReturnsAsync(false);

            var result = await _controller.DeleteDepartment("1");

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("not found", notFound.Value.ToString());
        }

        [Fact]
        public async Task DeleteDepartment_InvalidId_ReturnsBadRequest()
        {
            _mockService.Setup(s => s.DeleteDepartmentAsync("bad"))
                        .ThrowsAsync(new ArgumentException("Invalid delete request."));

            var result = await _controller.DeleteDepartment("bad");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteDepartment_Exception_Returns500()
        {
            _mockService.Setup(s => s.DeleteDepartmentAsync("1"))
                        .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _controller.DeleteDepartment("1");

            var error = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, error.StatusCode);
        }
    }
}
