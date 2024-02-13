using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPropertyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BookingPropertyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetBookingProperty")]
        public List<BookingProperty> GetBookingProperty()
        {
            return LoadListFromDb();
        }


        [HttpPost]
        [Route("PostBookingProperty")]
        public async Task<string> PostBookingProperty(BookingProperty bookingProperty)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "INSERT INTO BookingProperty (postId, fullName, email,address) VALUES (@postId, @fullName, @email,@address)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@postId", bookingProperty.postId);
                    cmd.Parameters.AddWithValue("@fullName", bookingProperty.fullName);
                    cmd.Parameters.AddWithValue("@email", bookingProperty.email);
                    cmd.Parameters.AddWithValue("@address", bookingProperty.address);
                    cmd.ExecuteNonQuery();
                }
                return "Booking Property added successfully";
            }
        }

        [HttpGet]
        [Route("CheckIfPropertyBooked")]
        public async Task<bool> CheckIfPropertyBooked(string userEmail, int postId)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM BookingProperty WHERE postId = @postId AND email = @email";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@email", userEmail);
                    int count = (int)cmd.ExecuteScalar();

                    // If count is greater than 0, property is booked by the user
                    return count > 0;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteBookingProperty")]
        public string DeleteBookingProperty(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM BookingProperty WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "BookingProperty deleted successfully";
            }
        }



        private List<BookingProperty> LoadListFromDb()
        {
            List<BookingProperty> LstMain = new List<BookingProperty>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from BookingProperty", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BookingProperty obj = new BookingProperty();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.postId = int.Parse(dt.Rows[i]["postId"].ToString());
                obj.fullName= dt.Rows[i]["fullName"].ToString();
                obj.email = dt.Rows[i]["email"].ToString();
                obj.address = dt.Rows[i]["address"].ToString();
                LstMain.Add(obj);
            }

            return LstMain;
        }
    }
}
