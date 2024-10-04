using System;

namespace TimeSheet.Models
{
    public class TimesheetEntry
    {
        public int Id { get; set; } // Primary Key
        public string EmployeeId { get; set; } // Employee ID for user
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string HoursWorked { get; set; }
    }
}
