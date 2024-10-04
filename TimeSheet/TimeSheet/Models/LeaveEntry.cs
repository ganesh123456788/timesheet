using System;

namespace TimeSheet.Models
{
    public class LeaveEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double EarnedLeaves { get; set; }
        public double PaidLeaves { get; set; }
        public int EmployeeId { get; set; } // Store EmployeeId
    }
}
