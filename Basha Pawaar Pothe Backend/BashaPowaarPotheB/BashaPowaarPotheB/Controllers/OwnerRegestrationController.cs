using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerRegestrationController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public OwnerRegestrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetOwner")]
        public List<OwnerRegister> GetOwner()
        {
            return LoadListFromDb();
        }

        [HttpGet]
        [Route("GetOwnerById")]
        public List<OwnerRegister> GetOwnerById(int ownerId)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.id == ownerId).ToList();

        }



        [HttpGet]
        [Route("GetOwnerByEmail")]
        public List<OwnerRegister> GetOwnerByEmail(string ownermail)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDbViewProfile().Where(e => e.email == ownermail).ToList();

        }




        [HttpPost]
        [Route("PostOwner")]
        public IActionResult PostOwner(OwnerRegister ownerRegister)
        {
            if (IsUserExists(ownerRegister.email, ownerRegister.phoneNumber))  //exists check
            {
                //return "Email or password already exists. Please use different credentials.";
                return BadRequest("Email or Phone number already exists. Please use different credentials.");
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                // Hash the password before storing it in the database for security
                //string hashedPassword = HashPassword(teenantRegister.password);

                string sql = "INSERT INTO OwnerReg (fullName, email, password, phoneNumber, address) VALUES (@fullName, @email, @password, @phoneNumber, @address)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fullName", ownerRegister.fullName);
                    cmd.Parameters.AddWithValue("@email", ownerRegister.email);
                    //cmd.Parameters.AddWithValue("@password", hashedPassword); // Store hashed password
                    cmd.Parameters.AddWithValue("@password", ownerRegister.password);
                    cmd.Parameters.AddWithValue("@phoneNumber", ownerRegister.phoneNumber);
                    cmd.Parameters.AddWithValue("@address", ownerRegister.address);

                    cmd.ExecuteNonQuery();
                }
                //return "Owner added successfully";
                return Ok("Owner added successfully");
            }
        }

        private bool IsUserExists(string email, string phoneNumber)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM OwnerReg WHERE email = @email OR phoneNumber = @phoneNumber";
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
        [Route("UpdateOwner")]

        public string UpdateOwner(int id, OwnerRegister ownerRegister)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "UPDATE OwnerReg SET fullName = @fullName, email = @email, password=@password, phoneNumber=@phoneNumber, address=@address  WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@fullName", ownerRegister.fullName);
                    cmd.Parameters.AddWithValue("@email", ownerRegister.email);
                    cmd.Parameters.AddWithValue("@password", ownerRegister.password);
                    cmd.Parameters.AddWithValue("@phoneNumber", ownerRegister.phoneNumber);
                    cmd.Parameters.AddWithValue("@address ", ownerRegister.address);

                    cmd.ExecuteNonQuery();
                }
                return "Owner updated successfully";
            }
        }

        [HttpDelete]
        [Route("DeleteOwner")]
        public string DeleteOwner(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM OwnerReg WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "Owner deleted successfully";
            }
        }

        private List<OwnerRegister> LoadListFromDb()
        {
            List<OwnerRegister> LstMain = new List<OwnerRegister>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from OwnerReg", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OwnerRegister obj = new OwnerRegister();
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




        private List<OwnerRegister> LoadListFromDbViewProfile()
        {
            List<OwnerRegister> LstMain = new List<OwnerRegister>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from OwnerReg", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OwnerRegister obj = new OwnerRegister();
                //obj.id = int.Parse(dt.Rows[i]["id"].ToString());
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
        [Route("OwnerLogin")]
        public IActionResult TeenantLogin(OwnerLoginRequest loginRequest)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM OwnerReg WHERE email = @email AND password = @password";
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
