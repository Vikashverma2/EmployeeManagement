using EmployeeManagement.Models;

namespace EmployeeManagement.Services.Interface
{
    public interface IDepartmentService
    {
        Task<Department> CreateDepartmentAsync(Department department);
        Task<List<Department>> GetDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(string id);
        Task<Department> UpdateDepartmentAsync(string id, Department updatedDepartment);
        Task<bool> DeleteDepartmentAsync(string id);
    }
}
