using System;
using System.Collections.Generic;

namespace TimeSheet.Models
{
    public class TimesheetViewModel
    {
        public DateTime Date { get; set; }
        public string Shift { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double HoursWorked { get; set; }
    }
}
