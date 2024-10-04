using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class TimesheetOverviewController : Controller
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: TimesheetOverview
        public async Task<ActionResult> Index()
        {
            List<Timesheet> timesheetList = new List<Timesheet>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Timesheets";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Timesheet timesheet = new Timesheet
                        {
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Task = reader["Task"].ToString(),
                            PendingTasks = reader["PendingTasks"].ToString(),
                            EmployeeId = reader["EmployeeId"].ToString()
                        };

                        timesheetList.Add(timesheet);
                    }
                }
            }

            return View(timesheetList);
        }

        // POST: TimesheetOverview/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string employeeId, DateTime date)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Timesheets WHERE EmployeeId = @EmployeeId AND Date = @Date";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@Date", date);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // POST: TimesheetOverview/ClearAll
        [HttpPost]
        public async Task<ActionResult> ClearAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Timesheets";

                using (var command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
