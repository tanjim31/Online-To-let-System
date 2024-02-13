using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeenantBooked2Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TeenantBooked2Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetTeenantBooked2")]
        public List<TeenantBooked2> GetTeenantBooked2()
        {
            return LoadListFromDb();
        }

        [HttpPost]
        [Route("PostTeenantBooked2")]
        public async Task<string> PostTeenantBooked2(TeenantBooked2 teenantBooked2)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "INSERT INTO TeenantBookedTbl2 (postId, teenantEmail, ownerEmail) VALUES (@postId, @teenantEmail, @ownerEmail)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@postId", teenantBooked2.postId);
                    cmd.Parameters.AddWithValue("@teenantEmail", teenantBooked2.teenantEmail);
                    cmd.Parameters.AddWithValue("@ownerEmail", teenantBooked2.ownerEmail);
                    cmd.ExecuteNonQuery();
                }
                return "TeenantBooked2 added successfully";
            }
        }

        //For checking

        [HttpGet]
        [Route("CheckIfPropertyBooked2")]
        public async Task<bool> CheckIfPropertyBooked2(string userEmail, string userEmail2, int postId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM TeenantBookedTbl2 WHERE postId = @postId AND teenantEmail = @teenantEmail AND ownerEmail= @ownerEmail";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@teenantEmail", userEmail);
                    cmd.Parameters.AddWithValue("@ownerEmail", userEmail2);
                    int count = (int)cmd.ExecuteScalar();

                    // If count is greater than 0, property is booked by the user
                    return count > 0;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteTeenantBooked2")]
        public string DeleteTeenantBooked2(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM TeenantBookedTbl2 WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "TeenantBooked2 deleted successfully";
            }
        }






        private List<TeenantBooked2> LoadListFromDb()
        {
            List<TeenantBooked2> LstMain = new List<TeenantBooked2>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from TeenantBookedTbl2", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TeenantBooked2 obj = new TeenantBooked2();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.postId = int.Parse(dt.Rows[i]["postId"].ToString());
                obj.teenantEmail = dt.Rows[i]["teenantEmail"].ToString();
                obj.ownerEmail = dt.Rows[i]["ownerEmail"].ToString();
                LstMain.Add(obj);
            }

            return LstMain;
        }
    }
}
