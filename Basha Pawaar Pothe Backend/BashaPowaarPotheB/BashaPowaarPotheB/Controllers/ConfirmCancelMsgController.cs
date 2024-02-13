using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmCancelMsgController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ConfirmCancelMsgController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetConfirmCancelMsg")]
        public List<ConfirmCancelMsg> GetConfirmCancelMsg()
        {
            return LoadListFromDb();
        }

        [HttpGet]
        [Route("GetConfirmCancelMsgByPostNo")]
        public List<ConfirmCancelMsg> GetConfirmCancelMsgByPostNo(int postId)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.postNo == postId).ToList();

        }


        [HttpPost]
        [Route("PostConfirmCancelMsg")]
        public async Task<string> PostConfirmCancelMsg(ConfirmCancelMsg confirmCancelMsg)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "INSERT INTO ConfirmCancelMsg (postNo, teenantMail, ownerMail, confirmMsg, deleteMsg) VALUES (@postNo, @teenantMail, @ownerMail, @confirmMsg, @deleteMsg)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@postNo", confirmCancelMsg.postNo);
                    cmd.Parameters.AddWithValue("@teenantMail", confirmCancelMsg.teenantMail);
                    cmd.Parameters.AddWithValue("@ownerMail", confirmCancelMsg.ownerMail);
                    cmd.Parameters.AddWithValue("@confirmMsg", confirmCancelMsg.confirmMsg);
                    cmd.Parameters.AddWithValue("@deleteMsg", confirmCancelMsg.deleteMsg);
                    cmd.ExecuteNonQuery();
                }
                return "ConfirmCancelMsg added successfully";
            }
        }

   

        [HttpDelete]
        [Route("DeleteConfirmCancelMsg")]
        public string DeleteConfirmCancelMsg(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM ConfirmCancelMsg WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "ConfirmCancelMsg deleted successfully";
            }
        }



        private List<ConfirmCancelMsg> LoadListFromDb()
        {
            List<ConfirmCancelMsg> LstMain = new List<ConfirmCancelMsg>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from ConfirmCancelMsg", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ConfirmCancelMsg obj = new ConfirmCancelMsg();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.postNo = int.Parse(dt.Rows[i]["postNo"].ToString());
                obj.teenantMail = dt.Rows[i]["teenantMail"].ToString();
                obj.ownerMail = dt.Rows[i]["ownerMail"].ToString();
                obj.confirmMsg = dt.Rows[i]["confirmMsg"].ToString();
                obj.deleteMsg = dt.Rows[i]["deleteMsg"].ToString();
                LstMain.Add(obj);
            }

            return LstMain;
        }
    }
}
