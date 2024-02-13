using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public ContactMessageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetMessage")]
        public List<ContactMessage> GetTeenant()
        {
            return LoadListFromDb();
        }

        [HttpGet]
        [Route("GetMessageById")]
        public List<ContactMessage> GetMessageById(int messageId)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.id == messageId).ToList();

        }

        [HttpPost]
        [Route("PostMessage")]

        public string PostMessage(ContactMessage contactMessage)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "INSERT INTO ContactMsg (firstName, lastName, phoneNumber, email, message) VALUES (@firstName, @lastName, @phoneNumber, @email, @message)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@firstName", contactMessage.firstName);
                    cmd.Parameters.AddWithValue("@lastName", contactMessage.lastName);
                    cmd.Parameters.AddWithValue("@phoneNumber", contactMessage.phoneNumber);
                    cmd.Parameters.AddWithValue("@email", contactMessage.email);
                    cmd.Parameters.AddWithValue("@message", contactMessage.message);

                    cmd.ExecuteNonQuery();
                }
                return "Message added successfully";
            }
        }

        [HttpPut]
        [Route("UpdateMessage")]

        public string UpdateMessage(int id, ContactMessage contactMessage)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "UPDATE ContactMsg SET firstName = @firstName, lastName = @lastName, phoneNumber=@phoneNumber, email=@email, message=@message  WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@firstName", contactMessage.firstName);
                    cmd.Parameters.AddWithValue("@lastName", contactMessage.lastName);
                    cmd.Parameters.AddWithValue("@phoneNumber", contactMessage.phoneNumber);
                    cmd.Parameters.AddWithValue("@email", contactMessage.email);
                    cmd.Parameters.AddWithValue("@message ", contactMessage.message);

                    cmd.ExecuteNonQuery();
                }
                return "Message updated successfully";
            }
        }

        [HttpDelete]
        [Route("DeleteMessage")]
        public string DeleteMessage(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM ContactMsg WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "Message deleted successfully";
            }
        }

        private List<ContactMessage> LoadListFromDb()
        {
            List<ContactMessage> LstMain = new List<ContactMessage>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from ContactMsg", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ContactMessage obj = new ContactMessage();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.firstName = dt.Rows[i]["firstName"].ToString();
                obj.lastName = dt.Rows[i]["lastName"].ToString();
                obj.email = dt.Rows[i]["email"].ToString();
                obj.phoneNumber = dt.Rows[i]["phoneNumber"].ToString();
                obj.message = dt.Rows[i]["message"].ToString();

                LstMain.Add(obj);
            }

            return LstMain;
        }
    }
}
