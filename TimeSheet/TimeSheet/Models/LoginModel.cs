using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public class LoginModel
    {
        [Required]
        public string EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
