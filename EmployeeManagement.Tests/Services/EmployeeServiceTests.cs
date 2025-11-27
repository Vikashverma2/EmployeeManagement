using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_mockRepo.Object);
        }


        [Fact]
        public async Task CreateEmployee_ShouldThrowArgumentException_WhenNameMissing()
        {
            var emp = new Employee { FullName = "", Email = "viku@gmail.com" };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(emp));

        }

        [Fact]
        public async Task CreateEmployee_ShouldThrowArgumentException_WhenEmailMissing()
        {
            var emp = new Employee { FullName = "Vikash Verma", Email = "" };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(emp));
        }

        [Fact]
        public async Task CreateEmployee_ShouldThrowInvalidOperationException_WhenEmailDuplicate()
        {
            var emp = new Employee { FullName = "Vikash Verma", Email = "viku@gmail.com" };

            var existing = new List<Employee>
            {
                new Employee { Email = "viku@gmail.com" }
            };

            _mockRepo.Setup(r => r.GetEmployeeAsync()).ReturnsAsync(existing);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateEmployeeAsync(emp));

            _mockRepo.Verify();
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnCreatedEmployee_WhenValid()
        {
            var emp = new Employee { FullName = "Vikash Verma", Email = "viku@gmail.com" };

            _mockRepo.Setup(r => r.GetEmployeeAsync()).ReturnsAsync(new List<Employee>());
            _mockRepo.Setup(r => r.CreateEmployeeAsync(emp)).ReturnsAsync(emp);

            var result = await _service.CreateEmployeeAsync(emp);

            Assert.Equal("Vikash Verma", result.FullName);

            _mockRepo.Verify();
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnList()
        {
            var list = new List<Employee> { new Employee { FullName = "Vikash Verma" } };

            _mockRepo.Setup(r => r.GetEmployeeAsync()).ReturnsAsync(list);

            var result = await _service.GetEmployeesAsync();

            Assert.Single(result);
            Assert.Equal("Vikash Verma", result[0].FullName);

            _mockRepo.Verify();
        }



        [Fact]
        public async Task GetEmployeeById_ShouldThrowArgumentException_WhenIdEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetEmployeeByIdAsync(""));
        }

        [Fact]
        public async Task GetEmployeeById_ShouldThrowKeyNotFound_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync("1")).ReturnsAsync((Employee)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetEmployeeByIdAsync("1"));

            _mockRepo.Verify();
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnEmployee_WhenExists()
        {
            var emp = new Employee { Id = "1", FullName = "Vikash Verma" };

            _mockRepo.Setup(r => r.GetEmployeeByIdAsync("1")).ReturnsAsync(emp);

            var result = await _service.GetEmployeeByIdAsync("1");

            Assert.Equal("Vikash Verma", result.FullName);

            _mockRepo.Verify();
        }


        [Fact]
        public async Task UpdateEmployee_ShouldThrowArgumentException_WhenIdEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateEmployeeAsync("", new Employee()));
        }

        [Fact]
        public async Task UpdateEmployee_ShouldThrowKeyNotFound_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync("1")).ReturnsAsync((Employee)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateEmployeeAsync("1", new Employee()));

            _mockRepo.Verify();
        }

        [Fact]
        public async Task UpdateEmployee_ShouldReturnUpdatedEmployee_WhenValid()
        {
            var existing = new Employee { Id = "1", FullName = "Old", Email = "old@mail.com" };
            var updated = new Employee { FullName = "New", Email = "new@mail.com", Salary = 20000 };

            _mockRepo.Setup(r => r.GetEmployeeByIdAsync("1")).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateEmployeeAsync("1", It.IsAny<Employee>()))
                     .ReturnsAsync(updated);

            var result = await _service.UpdateEmployeeAsync("1", updated);

            Assert.Equal("New", result.FullName);
            Assert.Equal("new@mail.com", result.Email);

            _mockRepo.Verify();
        }


        [Fact]
        public async Task DeleteEmployee_ShouldThrowArgumentException_WhenIdEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteEmployeeAsync(""));
        }

        [Fact]
        public async Task DeleteEmployee_ShouldThrowKeyNotFound_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync("1")).ReturnsAsync((Employee)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteEmployeeAsync("1"));

            _mockRepo.Verify();
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnTrue_WhenDeleted()
        {
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync("1"))
                     .ReturnsAsync(new Employee { Id = "1" });

            _mockRepo.Setup(r => r.DeleteEmployeeAsync("1")).ReturnsAsync(true);

            var result = await _service.DeleteEmployeeAsync("1");

            Assert.True(result);

            _mockRepo.Verify();
        }
    }
}
