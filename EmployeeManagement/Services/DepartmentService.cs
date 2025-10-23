using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Services
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

       
        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            //  Check for empty or null name
            if (string.IsNullOrWhiteSpace(department.Name))
                throw new ArgumentException("Department name is required.");

            //  Check for duplicate department name
            var allDepartments = await _departmentRepository.GetDepartmentAsync();
            bool exists = allDepartments.Any(d => d.Name.ToLower() == department.Name.ToLower());
            if (exists)
                throw new InvalidOperationException("Department with this name already exists.");

            return await _departmentRepository.CreateDepartmentAsync(department);
        }

    
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _departmentRepository.GetDepartmentAsync();
        }

     
        public async Task<Department> GetDepartmentByIdAsync(string id)
        {
            //  Check if ID is empty
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Department ID is required.");

            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException("Department not found.");

            return department;
        }

        
        public async Task<Department> UpdateDepartmentAsync(string id, Department updatedDepartment)
        {
            //  Check ID is valid
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Department ID is required.");

            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (existingDepartment == null)
                throw new KeyNotFoundException("Department not found.");

            // Check if name is empty
            if (string.IsNullOrWhiteSpace(updatedDepartment.Name))
                throw new ArgumentException("Department name is required.");

            return await _departmentRepository.UpdateDepartmentAsync(id, updatedDepartment);
        }

   
        public async Task<bool> DeleteDepartmentAsync(string id)
        {
            // Check ID is valid
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Department ID is required.");

            var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (existingDepartment == null)
                throw new KeyNotFoundException("Department not found.");

            return await _departmentRepository.DeleteDepartmentAsync(id);
        }
    }
}
