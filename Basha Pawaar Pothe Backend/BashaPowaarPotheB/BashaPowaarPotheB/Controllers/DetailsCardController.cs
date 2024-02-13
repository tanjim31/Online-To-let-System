using BashaPowaarPotheB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BashaPowaarPotheB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsCardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DetailsCardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetDetaills")]
        public List<DetailsCard> GetDetails()
        {
            return LoadListFromDb();
        }

        [HttpGet]
        [Route("GetDetailsSearch")]
        public List<DetailsCard> GetDetailsSearch(string searchAddress)
        {
            List<DetailsCard> allDetails = LoadListFromDb();

            if (!string.IsNullOrEmpty(searchAddress))
            {
                // Filter the list based on the searchAddress
                allDetails = allDetails.Where(d => d.address.Contains(searchAddress)).ToList();
            }

            return allDetails;
        }




        [HttpGet]
        [Route("GetDetailsById")]
        public List<DetailsCard> GetDetailsById(int detailsId)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.id == detailsId).ToList();

        }


        [HttpGet]
        [Route("GetDetailsByEmail")]
        public List<DetailsCard> GetDetailsByEmail(string detailsEmail)
        {
            //return LoadList().Where(e=>e.ItemCode==itemId).ToList();
            return LoadListFromDb().Where(e => e.userEmail == detailsEmail).ToList();

        }


        [HttpPost]
        [Route("PostDetails")]
        public async Task<string> PostDetails([FromForm] DetailsCard detailsCard)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "INSERT INTO DetailsCardstbl (category, homeType, address, date, price, floor, bedroom, bathroom, contactNumber, description, review, userEmail, image) VALUES (@category, @homeType, @address, @date, @price, @floor, @bedroom, @bathroom, @contactNumber, @description, @review, @userEmail, CAST(@image AS VARBINARY(MAX)))";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@category", detailsCard.category);
                    cmd.Parameters.AddWithValue("@homeType", detailsCard.homeType);
                    cmd.Parameters.AddWithValue("@address", detailsCard.address);
                    cmd.Parameters.AddWithValue("@date", detailsCard.date);
                    cmd.Parameters.AddWithValue("@price", detailsCard.price);
                    cmd.Parameters.AddWithValue("@floor", detailsCard.floor);
                    cmd.Parameters.AddWithValue("@bedroom", detailsCard.bedroom);
                    cmd.Parameters.AddWithValue("@bathroom", detailsCard.bathroom);
                    cmd.Parameters.AddWithValue("@contactNumber", detailsCard.contactNumber);
                    cmd.Parameters.AddWithValue("@description", detailsCard.description);
                    cmd.Parameters.AddWithValue("@review", detailsCard.review);
                    cmd.Parameters.AddWithValue("@userEmail", detailsCard.userEmail);
                    //cmd.Parameters.AddWithValue("@image", detailsCard.image ?? (object)DBNull.Value);
                    // ... (other parameters)

                    if (detailsCard.image != null && detailsCard.image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await detailsCard.image.CopyToAsync(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            cmd.Parameters.AddWithValue("@image", imageBytes);
                            Console.WriteLine(detailsCard.image);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@image", DBNull.Value);
                        Console.WriteLine(detailsCard.image);
                    }

                    //// Assuming detailsCard is an instance of your DetailsCard class
                    //// Assuming detailsCard is an instance of your DetailsCard class
                    //cmd.Parameters.Add("@imageField", SqlDbType.VarBinary).Value = detailsCard.imageField ?? (object)DBNull.Value;



                    cmd.ExecuteNonQuery();
                }
                return "Details added successfully";
            }
        }





        [HttpPut]
        [Route("UpdateDetails")]
        public async Task<string> UpdateDetails(int id, [FromForm] DetailsCard detailsCard)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "UPDATE DetailsCardstbl SET category = @category, homeType = @homeType, address=@address, date=@date, price=@price, floor=@floor, bedroom=@bedroom, bathroom=@bathroom, contactNumber=@contactNumber, description=@description, review=@review";

                // Check if the image is provided for update
                if (detailsCard.image != null && detailsCard.image.Length > 0)
                {
                    sql += ", image=@image";
                }

                sql += " WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@category", detailsCard.category);
                    cmd.Parameters.AddWithValue("@homeType", detailsCard.homeType);
                    cmd.Parameters.AddWithValue("@address", detailsCard.address);
                    cmd.Parameters.AddWithValue("@date", detailsCard.date);
                    cmd.Parameters.AddWithValue("@price", detailsCard.price);
                    cmd.Parameters.AddWithValue("@floor", detailsCard.floor);
                    cmd.Parameters.AddWithValue("@bedroom", detailsCard.bedroom);
                    cmd.Parameters.AddWithValue("@bathroom", detailsCard.bathroom);
                    cmd.Parameters.AddWithValue("@contactNumber", detailsCard.contactNumber);
                    cmd.Parameters.AddWithValue("@description", detailsCard.description);
                    cmd.Parameters.AddWithValue("@review", detailsCard.review);

                    // Update the image if provided
                    if (detailsCard.image != null && detailsCard.image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await detailsCard.image.CopyToAsync(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            cmd.Parameters.AddWithValue("@image", imageBytes);
                        }
                    }

                    cmd.ExecuteNonQuery();
                }

                return "Details updated successfully";
            }
        }


   


        [HttpPut]
        [Route("UpdateDetailsByEmail")]

        public string UpdateDetailsByEmail([FromForm]  string userEmail, DetailsCard detailsCard)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "UPDATE DetailsCardstbl SET category = @category, homeType = @homeType, address=@address, date=@date, price=@price, floor=@floor, bedroom=@bedroom, bathroom=@bathroom, contactNumber=@contactNumber, description=@description, review=@review   WHERE userEmail = @userEmail  AND  id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", detailsCard.id);
                    cmd.Parameters.AddWithValue("@category", detailsCard.category);
                    cmd.Parameters.AddWithValue("@homeType", detailsCard.homeType);
                    cmd.Parameters.AddWithValue("@address", detailsCard.address);
                    cmd.Parameters.AddWithValue("@date", detailsCard.date);
                    cmd.Parameters.AddWithValue("@price", detailsCard.price);
                    cmd.Parameters.AddWithValue("@floor", detailsCard.floor);
                    cmd.Parameters.AddWithValue("@bedroom", detailsCard.bedroom);
                    cmd.Parameters.AddWithValue("@bathroom", detailsCard.bathroom);
                    cmd.Parameters.AddWithValue("@contactNumber", detailsCard.contactNumber);
                    cmd.Parameters.AddWithValue("@description", detailsCard.description);
                    cmd.Parameters.AddWithValue("@review", detailsCard.review);
                    cmd.Parameters.AddWithValue("@userEmail", detailsCard.userEmail);


                    cmd.ExecuteNonQuery();
                }
                return "Details updated successfully";
            }
        }




        [HttpDelete]
        [Route("DeleteDetails")]
        public string DeleteDetails(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM DetailsCardstbl WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                return "Details deleted successfully";
            }
        }



        [HttpDelete]
        [Route("DeleteDetailsIdEmail")]
        public string DeleteDetailsIdEmail(int id, string userEmail)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                string sql = "DELETE FROM DetailsCardstbl WHERE id = @id AND userEmail = @userEmail";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userEmail", userEmail);

                    cmd.ExecuteNonQuery();
                }

                return "Details deleted successfully";
            }
        }

      



        private List<DetailsCard> LoadListFromDb()
        {
            List<DetailsCard> LstMain = new List<DetailsCard>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from DetailsCardstbl", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DetailsCard obj = new DetailsCard();
                obj.id = int.Parse(dt.Rows[i]["id"].ToString());
                obj.category = dt.Rows[i]["category"].ToString();
                obj.homeType = dt.Rows[i]["homeType"].ToString();
                obj.address = dt.Rows[i]["address"].ToString();
                obj.date = DateTime.Parse(dt.Rows[i]["date"].ToString());
                obj.price = decimal.Parse(dt.Rows[i]["price"].ToString());
                obj.floor = int.Parse(dt.Rows[i]["floor"].ToString());
                obj.bedroom = int.Parse(dt.Rows[i]["bedroom"].ToString());
                obj.bathroom = int.Parse(dt.Rows[i]["bathroom"].ToString());
                obj.contactNumber = dt.Rows[i]["contactNumber"].ToString();
                obj.description = dt.Rows[i]["description"].ToString();
                obj.review = dt.Rows[i]["review"].ToString();
                obj.userEmail = dt.Rows[i]["userEmail"].ToString();
               
                obj.imageField = ConvertToByteArray(dt.Rows[i]["image"]);

                LstMain.Add(obj);
            }

            return LstMain;
        }

        private byte[] ConvertToByteArray(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            try
            {
                return (byte[])value;
            }
            catch (InvalidCastException)
            {
                // Handle the case where the value cannot be cast to byte array
                return null;
            }
        }
    }
}
