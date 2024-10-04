using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class TaskController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True;";

        // GET: Task
        public ActionResult Index()
        {
            List<Timesheet> timesheetList = new List<Timesheet>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Timesheets";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Timesheet timesheet = new Timesheet
                    {
                        Date = Convert.ToDateTime(reader["Date"]),
                        Task = reader["Task"].ToString(),
                        PendingTasks = reader["PendingTasks"].ToString(),
                        EmployeeId = reader["EmployeeId"].ToString()  // Retrieve EmployeeId
                    };

                    timesheetList.Add(timesheet);
                }

                connection.Close();
            }

            return View(timesheetList);
        }

        // POST: Task/Add
        [HttpPost]
        public ActionResult Add(Timesheet timesheet)
        {
            // Get the EmployeeId from the session
            string employeeId = Session["EmployeeId"] as string;

            if (string.IsNullOrEmpty(employeeId))
            {
                // If session is null, redirect to login
                return RedirectToAction("Login", "Account");
            }

            // Assign the EmployeeId to the Timesheet model
            timesheet.EmployeeId = employeeId;

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Timesheets (Date, Task, PendingTasks, EmployeeId) VALUES (@Date, @Task, @PendingTasks, @EmployeeId)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Date", timesheet.Date);
                    command.Parameters.AddWithValue("@Task", timesheet.Task);
                    command.Parameters.AddWithValue("@PendingTasks", timesheet.PendingTasks);
                    command.Parameters.AddWithValue("@EmployeeId", timesheet.EmployeeId);  // Store EmployeeId

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return RedirectToAction("Index");
            }

            return View(timesheet);
        }
    }
}
