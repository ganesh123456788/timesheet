using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSheet.Models
{
    public class Leave
    {
        public int Id { get; set; } // If you use Id, change as per your preference.
        public string LeaveType { get; set; }
        public string LeaveDates { get; set; }
        public string Status { get; set; }
        public string EmployeeId { get; set; } // New property for Employee ID
    }

}