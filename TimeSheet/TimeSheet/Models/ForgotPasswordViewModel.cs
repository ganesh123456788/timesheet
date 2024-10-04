using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public string EmployeeId { get; set; }
    }
}
