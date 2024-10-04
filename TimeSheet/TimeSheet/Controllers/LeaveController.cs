using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

public class LeaveController : Controller
{
    private readonly string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True"; // Set your connection string

    // GET: Leave
    public ActionResult Index()
    {
        List<LeaveEntry> leaveEntries = GetLeaveEntries();
        return View(leaveEntries);
    }

    // POST: Leave
    [HttpPost]
    public ActionResult Index(LeaveEntry entry)
    {
        if (ModelState.IsValid)
        {
            int employeeId;
            if (Session["EmployeeId"] != null && int.TryParse(Session["EmployeeId"].ToString(), out employeeId))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string insertQuery = "INSERT INTO LeaveEntries (Date, EarnedLeaves, PaidLeaves, EmployeeId) VALUES (@Date, @EarnedLeaves, @PaidLeaves, @EmployeeId)";
                        SqlCommand command = new SqlCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@Date", entry.Date);
                        command.Parameters.AddWithValue("@EarnedLeaves", entry.EarnedLeaves);
                        command.Parameters.AddWithValue("@PaidLeaves", entry.PaidLeaves);
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);

                        command.ExecuteNonQuery();
                        return RedirectToAction("Index"); // Redirect after successful insertion
                    }
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", "Error adding leave entry: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "User not logged in or invalid Employee ID.");
            }
        }

        // If model state is not valid or there is an error, reload the view
        List<LeaveEntry> leaveEntries = GetLeaveEntries();
        return View(leaveEntries);
    }

    private List<LeaveEntry> GetLeaveEntries()
    {
        List<LeaveEntry> leaveEntries = new List<LeaveEntry>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM LeaveEntries", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                leaveEntries.Add(new LeaveEntry
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    EarnedLeaves = (double)reader["EarnedLeaves"],
                    PaidLeaves = (double)reader["PaidLeaves"],
                    EmployeeId = (int)reader["EmployeeId"] // Make sure you have EmployeeId in the model
                });
            }
        }

        return leaveEntries;
    }
}
