using System;
using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public class Timesheet
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Task { get; set; }

        public string PendingTasks { get; set; }

        public string EmployeeId { get; set; }  // New Property for EmployeeId
    }
}
