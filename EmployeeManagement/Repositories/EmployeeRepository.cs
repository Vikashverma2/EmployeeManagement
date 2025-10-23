using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interface;
using MongoDB.Driver;

namespace EmployeeManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employee;

        public EmployeeRepository(IMongoDatabase database)
        {
            _employee = database.GetCollection<Employee>("Employees");
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            await _employee.InsertOneAsync(employee);
            return employee;
        }

        public async Task<List<Employee>> GetEmployeeAsync()
        {
            var getEmployee = await _employee.Find(a => true).ToListAsync();
            return getEmployee;

        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            return await _employee.Find(emp => emp.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Employee?> UpdateEmployeeAsync(string id, Employee updatedEmployee)
        {
            var existingEmployee = await _employee.Find(emp => emp.Id == id).FirstOrDefaultAsync();
            if (existingEmployee == null)
            {
                return null;
            }
            existingEmployee.FullName = updatedEmployee.FullName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Salary = updatedEmployee.Salary;

            await _employee.ReplaceOneAsync(emp => emp.Id == id, existingEmployee);
            return existingEmployee;
        }
        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            var delete = await _employee.DeleteOneAsync(id);
            return delete.DeletedCount>0;
        }





    }
}
