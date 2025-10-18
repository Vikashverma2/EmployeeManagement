using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.RequestModels
{
    public class DepartmentRequest
    {
        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; } = string.Empty;


    }
}
