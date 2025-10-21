using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.RequestModels
{
    public class EmployeeRequest
    {
        
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Salary { get; set; }


    }
}
