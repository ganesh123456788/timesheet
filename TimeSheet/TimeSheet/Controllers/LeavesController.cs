using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

public class LeavesController : Controller
{
    private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True";

    // GET: Leaves
    public ActionResult Index()
    {
        var leaves = GetLeaves(); // Retrieve leaves from the database
        return View(leaves); // Pass leaves to the view
    }

    // POST: SubmitLeave
    [HttpPost]
    public ActionResult SubmitLeave(Leave leave)
    {
        if (ModelState.IsValid)
        {
            leave.EmployeeId = Session["EmployeeId"]?.ToString();
            leave.Status = "Pending"; // Set initial status

            // Save leave entry to the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Leaves (LeaveType, LeaveDates, Status, EmployeeId) VALUES (@LeaveType, @LeaveDates, @Status, @EmployeeId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LeaveType", leave.LeaveType);
                cmd.Parameters.AddWithValue("@LeaveDates", leave.LeaveDates);
                cmd.Parameters.AddWithValue("@Status", leave.Status);
                cmd.Parameters.AddWithValue("@EmployeeId", leave.EmployeeId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Invalid data." });
    }

    // Method to retrieve leave records from the database
    private List<Leave> GetLeaves()
    {
        var leaves = new List<Leave>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT LeaveType, LeaveDates, Status, EmployeeId FROM Leaves";
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    leaves.Add(new Leave
                    {
                        LeaveType = reader["LeaveType"].ToString(),
                        LeaveDates = reader["LeaveDates"].ToString(),
                        Status = reader["Status"].ToString(),
                        EmployeeId = reader["EmployeeId"].ToString()
                    });
                }
            }
        }

        return leaves;
    }
}
