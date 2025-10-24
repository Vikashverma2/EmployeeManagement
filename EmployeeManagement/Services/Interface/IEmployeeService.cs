using EmployeeManagement.Models;

namespace EmployeeManagement.Services.Interface
{
    public interface IEmployeeService 
    {
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(string id);
        Task<Employee> UpdateEmployeeAsync(string id, Employee updatedEmployee);
        Task<bool> DeleteEmployeeAsync(string id);

    }
}
