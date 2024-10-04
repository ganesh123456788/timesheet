using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

public class LeaveRecordsController : Controller
{
    private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True";

    // GET: LeaveRecords
    public ActionResult Index()
    {
        var leaves = GetLeaves();
        return View(leaves);
    }

    // Method to retrieve leave records from the database
    private List<Leave> GetLeaves()
    {
        var leaves = new List<Leave>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT LeaveType, LeaveDates, Status, EmployeeId FROM Leaves";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
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
        }

        return leaves;
    }

    // POST: DeleteLeave
    [HttpPost]
    public ActionResult DeleteLeave(string employeeId, string leaveType)
    {
        try
        {
            DeleteLeaveRecord(employeeId, leaveType);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            // Log exception (consider using a logging framework)
            return Json(new { success = false, message = "An error occurred while deleting the leave." });
        }
    }

    // Method to delete a leave entry from the database
    private void DeleteLeaveRecord(string employeeId, string leaveType)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Leaves WHERE EmployeeId = @EmployeeId AND LeaveType = @LeaveType";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                cmd.Parameters.AddWithValue("@LeaveType", leaveType);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // POST: ClearAllLeaves
    [HttpPost]
    public ActionResult ClearAllLeaves()
    {
        try
        {
            ClearAllLeaveRecords();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            // Log exception (consider using a logging framework)
            return Json(new { success = false, message = "An error occurred while clearing all leaves." });
        }
    }

    // Method to delete all leave entries from the database
    private void ClearAllLeaveRecords()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Leaves";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
