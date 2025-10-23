using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interface
{
    public interface IEmployeeRepository
    {
         
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<List<Employee>> GetEmployeeAsync();
        Task<Employee?> GetEmployeeByIdAsync(string id);
        Task<Employee?> UpdateEmployeeAsync(string id, Employee updatedEmployee);
        Task<bool> DeleteEmployeeAsync(string id);

    }
}
