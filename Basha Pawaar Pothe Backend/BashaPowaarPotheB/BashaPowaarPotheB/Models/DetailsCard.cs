namespace BashaPowaarPotheB.Models
{
    public class DetailsCard
    {
        public int id {  get; set; }
        public string category { get; set; }
        public string homeType { get; set; }
        public string address { get; set; }
        public DateTime date { get; set; }
        public decimal price { get; set; }
        public int floor { get; set; }
        public int bedroom { get; set; }
        public int bathroom { get; set; }
        public string contactNumber { get; set; }
        public string description { get; set; }
        public string review { get; set; }
        public IFormFile image { get; set; }

        public string userEmail { get; set; }
        public byte[]? imageField { get; set; }

    }
}
