using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class TimesheetDisplayController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True";

        // GET: TimesheetDisplay
        public ActionResult Index()
        {
            var entries = GetTimesheetEntries();
            return View(entries);
        }

        // Action to delete a timesheet entry by EmployeeId and Date
        [HttpPost]
        public ActionResult Delete(string employeeId, DateTime date)
        {
            DeleteTimesheetEntry(employeeId, date);
            return RedirectToAction("Index");
        }

        // Action to clear all timesheet entries
        [HttpPost]
        public ActionResult ClearAllEntries()
        {
            ClearTimesheetEntries();
            return RedirectToAction("Index");
        }

        private List<TimesheetEntry> GetTimesheetEntries()
        {
            var entries = new List<TimesheetEntry>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT EmployeeId, Date, StartTime, EndTime, HoursWorked FROM Timesheet";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entries.Add(new TimesheetEntry
                            {
                                EmployeeId = reader["EmployeeId"].ToString(),
                                Date = (DateTime)reader["Date"],
                                StartTime = reader["StartTime"].ToString(),
                                EndTime = reader["EndTime"].ToString(),
                                HoursWorked = reader["HoursWorked"].ToString()
                            });
                        }
                    }
                }
            }
            return entries;
        }

        private void DeleteTimesheetEntry(string employeeId, DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Timesheet WHERE EmployeeId = @EmployeeId AND Date = @Date";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ClearTimesheetEntries()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Timesheet";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
