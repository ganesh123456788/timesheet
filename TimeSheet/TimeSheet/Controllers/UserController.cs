using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class UserController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True";

        // GET: List of users
        public ActionResult Index()
        {
            List<User> usersList = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT EmployeeId, Name, Email, DateOfBirth, Gender, Address, Pincode FROM Employees";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usersList.Add(new User
                            {
                                EmployeeId = reader["EmployeeId"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = reader["Gender"].ToString(),
                                Address = reader["Address"].ToString(),
                                Pincode = reader["Pincode"].ToString()
                            });
                        }
                    }
                }
            }

            return View(usersList);
        }

        // POST: Delete User by EmployeeId
        [HttpPost]
        public ActionResult Delete(string employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle error or log the exception
                Console.WriteLine("Error: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal server error");
            }
        }
    }
}
