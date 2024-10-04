using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class TimesheetController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True";

        // GET: Timesheet
        public ActionResult Index()
        {
            var entries = GetTimesheetEntries();
            return View(entries);
        }

        // POST: Timesheet/AddEntry
        [HttpPost]
        public ActionResult AddEntry(TimesheetEntry entry)
        {
            if (ModelState.IsValid)
            {
                entry.EmployeeId = Session["EmployeeId"].ToString(); // Get Employee ID from session
                SaveTimesheetEntry(entry);
                return RedirectToAction("Index");
            }

            var entries = GetTimesheetEntries();
            return View("Index", entries);
        }

        private void SaveTimesheetEntry(TimesheetEntry entry)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Timesheet (EmployeeId, Date, StartTime, EndTime, HoursWorked) VALUES (@EmployeeId, @Date, @StartTime, @EndTime, @HoursWorked)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", entry.EmployeeId);
                    cmd.Parameters.AddWithValue("@Date", entry.Date);
                    cmd.Parameters.AddWithValue("@StartTime", entry.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", entry.EndTime);
                    cmd.Parameters.AddWithValue("@HoursWorked", entry.HoursWorked);
                    cmd.ExecuteNonQuery();
                }
            }
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
    }
}
