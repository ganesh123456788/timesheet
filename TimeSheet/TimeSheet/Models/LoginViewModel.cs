using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public class LoginViewModel
    {
        [Required]
        public string EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
