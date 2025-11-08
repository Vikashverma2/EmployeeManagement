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



    }
}