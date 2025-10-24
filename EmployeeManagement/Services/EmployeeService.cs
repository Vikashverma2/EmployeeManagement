using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Interface;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // ✅ Create Employee
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new ArgumentException("Employee name is required.");

            if (string.IsNullOrWhiteSpace(employee.Email))
                throw new ArgumentException("Employee email is required.");

            // Check for duplicate email
            var allEmployees = await _employeeRepository.GetEmployeeAsync();
            bool exists = allEmployees.Any(e => e.Email.ToLower() == employee.Email.ToLower());

            if (exists)
                throw new InvalidOperationException("Employee with this email already exists.");

            return await _employeeRepository.CreateEmployeeAsync(employee);
        }

        // ✅ Get All Employees
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeeAsync();
        }

        // ✅ Get Employee by ID
        public async Task<Employee> GetEmployeeByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Employee ID is required.");

            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            return employee;
        }

        // ✅ Update Employee
        public async Task<Employee> UpdateEmployeeAsync(string id, Employee updatedEmployee)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Employee ID is required.");

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                throw new KeyNotFoundException("Employee not found.");

            existingEmployee.FullName = updatedEmployee.FullName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Salary = updatedEmployee.Salary;

            return await _employeeRepository.UpdateEmployeeAsync(id, existingEmployee);
        }

        // ✅ Delete Employee
        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Employee ID is required.");

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                throw new KeyNotFoundException("Employee not found.");

            return await _employeeRepository.DeleteEmployeeAsync(id);
        }
    }
}
