using EmployeeManagement.Models;
using EmployeeManagement.MongoDb;
using EmployeeManagement.Repositories.Interface;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IMongoCollection<Department> _department;

        public DepartmentRepository(DbContext dbContext)
        {
            _department = dbContext.GetCollection<Department>("Departments");
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            await _department.InsertOneAsync(department);
            return department;      
        } 

        public async Task<List<Department>> GetDepartmentAsync()
        {
            var getDepartment = await _department.Find(a => true).ToListAsync();
            return getDepartment;
        }

        public async Task<Department?> GetDepartmentByIdAsync(string id)
        {
            return await _department.Find(dept => dept.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Department?> UpdateDepartmentAsync(string id, Department updatedDepartment)
        {
            var excitingDepartment = await _department.Find(dept => dept.Id == id).FirstOrDefaultAsync();

            excitingDepartment.Name = updatedDepartment.Name;
            await _department.ReplaceOneAsync(dept => dept.Id == id, excitingDepartment);
            return excitingDepartment;

        }

        public async Task<bool> DeleteDepartmentAsync(string id)
        {
            var result = await _department.DeleteOneAsync(dept => dept.Id == id);
            return result.DeletedCount > 0;
        }


    }
}
