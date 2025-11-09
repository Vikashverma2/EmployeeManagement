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
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> mockService;
        private readonly Mock<ILogger<DepartmentController>> mockLogger;
        private readonly DepartmentController controller;

        public DepartmentControllerTests()
        {
            mockService = new Mock<IDepartmentService>();
            mockLogger = new Mock<ILogger<DepartmentController>>();

            controller = new DepartmentController
            (
                mockService.Object,
                mockLogger.Object
            );
        }

        [Fact]
        public async Task GetAllDepartments_ReturnsOkResult_WithListOfDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = "1", Name = "HR" },
                new Department { Id = "2", Name = "IT" }
            };
            mockService.Setup(s => s.GetDepartmentsAsync())
                       .ReturnsAsync(departments);
            // Act
            var result = await controller.GetAllDepartments();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnDepartments = Assert.IsType<List<Department>>(okResult.Value);

            Assert.Equal(2, returnDepartments.Count);
        }

        [Fact]
        public async Task CreateDepartment_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var input = new Department();
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.CreateDepartment(input);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            // Verify 
            mockService.Verify(s => s.CreateDepartmentAsync(It.IsAny<Department>()), Times.Never);
        }







    }
}