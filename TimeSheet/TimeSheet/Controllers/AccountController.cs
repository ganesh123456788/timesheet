using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class AccountController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=exam;Integrated Security=True";

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Employees (EmployeeId, Name, Email, Password, DateOfBirth, Gender, Address, Pincode) " +
                                   "VALUES (@EmployeeId, @Name, @Email, @Password, @DateOfBirth, @Gender, @Address, @Pincode)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EmployeeId", user.EmployeeId);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", HashPassword(user.Password)); // Hash password
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Pincode", user.Pincode);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user.EmployeeId == "ETS0001" && user.Password == "admin")
            {
                Session["EmployeeId"] = user.EmployeeId;
                return RedirectToAction("Index", "Home");
            }

            if (IsValidUser(user.EmployeeId, user.Password))
            {
                Session["EmployeeId"] = user.EmployeeId;
                return RedirectToAction("About", "Home");
            }

            ModelState.AddModelError("", "Invalid Employee ID or Password");
            return View(user);
        }

        // Validate user login
        private bool IsValidUser(string employeeId, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Password FROM Employees WHERE EmployeeId = @EmployeeId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        var storedHash = (string)cmd.ExecuteScalar();
                        return storedHash != null && VerifyPasswordHash(password, storedHash);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // Password hashing
        private string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Verify password
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            string hashedInput = HashPassword(password);
            return hashedInput == storedHash;
        }
    }
}
