using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interface
{
    public interface IDepartmentRepository
    {

        Task<Department> CreateDepartmentAsync(Department department);
        Task<List<Department>> GetDepartmentAsync();
        Task<Department?> GetDepartmentByIdAsync(string id);
        Task<Department?> UpdateDepartmentAsync(string id, Department updatedDepartment);
        Task<bool> DeleteDepartmentAsync(string id);

    }
}
