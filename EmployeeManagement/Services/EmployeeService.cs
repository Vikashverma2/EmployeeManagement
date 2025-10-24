using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new ArgumentException("Employee name is required.");

            // Check for duplicate employee name
            var allEmployees = await _employeeRepository.GetEmployeeAsync();
            bool exists = allEmployees.Any(e => e.FullName.ToLower() == employee.FullName.ToLower());

            if (exists)
                throw new InvalidOperationException("Employee with this name already exists.");

            return await _employeeRepository.CreateEmployeeAsync(employee);
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeeAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Employee ID is required.");

            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            return employee;
        }

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

            await _employeeRepository.UpdateEmployeeAsync(id, existingEmployee);
            return existingEmployee;
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Employee ID is required.");

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                throw new KeyNotFoundException("Employee not found.");

            await _employeeRepository.DeleteEmployeeAsync(id);
        }
    }
}
