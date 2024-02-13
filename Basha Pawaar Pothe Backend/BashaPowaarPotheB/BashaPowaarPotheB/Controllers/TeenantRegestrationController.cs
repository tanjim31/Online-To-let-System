using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeenantRegestrationController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public TeenantRegestrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetTeenant")]
        public List<TeenantRegister> GetTeenant()
        {
            return LoadListFromDb();
        }

        [HttpGet]
        [Route("GetTeenantById")]
        public List<TeenantRegister> GetTeenantById(int teenantId)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.id == teenantId).ToList();

        }

        [HttpGet]
        [Route("GetTeenantByEmail")]
        public List<TeenantRegister> GetTeenantByEmail(string teenantmail)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.email == teenantmail).ToList();

        }




        [HttpPost]
        [Route("PostTeenant")]
        public IActionResult PostTeenant(TeenantRegister teenantRegister)
        {
            if (IsUserExists(teenantRegister.email, teenantRegister.phoneNumber))  //exists check
            {
                return BadRequest("Email or Phone Number already exists. Please use different credentials.");
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();


                string sql = "INSERT INTO TeenantReg (fullName, email, password, phoneNumber, address) VALUES (@fullName, @email, @password, @phoneNumber, @address)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fullName", teenantRegister.fullName);
                    cmd.Parameters.AddWithValue("@email", teenantRegister.email);
                    cmd.Parameters.AddWithValue("@password", teenantRegister.password);
                    cmd.Parameters.AddWithValue("@phoneNumber", teenantRegister.phoneNumber);
                    cmd.Parameters.AddWithValue("@address", teenantRegister.address);

                    cmd.ExecuteNonQuery();
                }
                return Ok("Owner added successfully");
            }
        }


        private bool IsUserExists(string email, string phoneNumber)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM TeenantReg WHERE email = @email OR phoneNumber = @phoneNumber";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        [HttpPut]
        [Route("UpdateTeenant")]

        public string UpdateTeenant(int id, TeenantRegister teenantRegister)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "UPDATE TeenantReg SET fullName = @fullName, email = @email, password=@password, phoneNumber=@phoneNumber, address=@address  WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@fullName", teenantRegister.fullName);
                    cmd.Parameters.AddWithValue("@email", teenantRegister.email);
                    cmd.Parameters.AddWithValue("@password", teenantRegister.password);
                    cmd.Parameters.AddWithValue("@phoneNumber", teenantRegister.phoneNumber);
                    cmd.Parameters.AddWithValue("@address ", teenantRegister.address);

                    cmd.ExecuteNonQuery();
                }
                return "Teenant updated successfully";
            }
        }

        [HttpDelete]
        [Route("DeleteTeenant")]
        public string DeleteTeenant(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM TeenantReg WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "Teenant deleted successfully";
            }
        }


        private List<TeenantRegister> LoadListFromDb()
        {
            List<TeenantRegister> LstMain = new List<TeenantRegister>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from TeenantReg", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TeenantRegister obj = new TeenantRegister();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.fullName = dt.Rows[i]["fullName"].ToString();
                obj.email = dt.Rows[i]["email"].ToString();
                obj.password = dt.Rows[i]["password"].ToString();
                obj.phoneNumber = dt.Rows[i]["phoneNumber"].ToString();
                obj.address = dt.Rows[i]["address"].ToString();

                LstMain.Add(obj);
            }

            return LstMain;
        }



        [HttpPost]
        [Route("TeenantLogin")]
        public IActionResult TeenantLogin(TeenantLoginRequest loginRequest)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM TeenantReg WHERE email = @email AND password = @password";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@email", loginRequest.Email);
                    cmd.Parameters.AddWithValue("@password", loginRequest.Password);
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
