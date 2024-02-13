using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AdminRegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAdmin")]
        public List<AdminRegistration> GetAdmin()
        {
            return LoadListFromDb();
        }

        [HttpGet]
        [Route("GetAdminById")]
        public List<AdminRegistration> GetAdminById(int adminId)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.id == adminId).ToList();

        }




        [HttpPost]
        [Route("PostAdmin")]
        public IActionResult PostAdmin(AdminRegistration adminRegistration)
        {
            if (IsUserExists(adminRegistration.email, adminRegistration.password))  //exists check
            {
                return BadRequest("Email or Password already exists. Please use different credentials.");
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();


                string sql = "INSERT INTO AdminRegistrations ( email, password) VALUES (@email, @password)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                 
                    cmd.Parameters.AddWithValue("@email", adminRegistration.email);
                    cmd.Parameters.AddWithValue("@password", adminRegistration.password);
               

                    cmd.ExecuteNonQuery();
                }
                return Ok("Owner added successfully");
            }
        }


        private bool IsUserExists(string email, string password)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM AdminRegistrations WHERE email = @email OR password = @password";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }



        [HttpDelete]
        [Route("DeleteAdmin")]
        public string DeleteAdmin(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM AdminRegistrations WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "Admin deleted successfully";
            }
        }






        private List<AdminRegistration> LoadListFromDb()
        {
            List<AdminRegistration> LstMain = new List<AdminRegistration>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from AdminRegistrations", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AdminRegistration obj = new AdminRegistration();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.email = dt.Rows[i]["email"].ToString();
                obj.password = dt.Rows[i]["password"].ToString();
             

                LstMain.Add(obj);
            }

            return LstMain;
        }




        [HttpPost]
        [Route("AdminLogin")]
        public IActionResult AdminLogin(AdminLogin adminLogin)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM AdminRegistrations WHERE email = @email AND password = @password";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@email", adminLogin.email);
                    cmd.Parameters.AddWithValue("@password", adminLogin.password);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Login successful, return a success message
                        return Ok(new { message = "Login successful" });
                    }
                    else
                    {
                        // Invalid email or password, return a failure message
                        return BadRequest(new { message = "Invalid email or password" });
                    }
                }
            }
        }
    }
}
