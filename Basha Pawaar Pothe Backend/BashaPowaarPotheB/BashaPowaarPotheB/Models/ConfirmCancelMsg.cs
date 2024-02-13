using System.Security.Cryptography.X509Certificates;

namespace BashaPowaarPotheB.Models
{
    public class ConfirmCancelMsg
    {
        public int id { get; set; }
        public int postNo { get; set; }
        public string teenantMail { get; set; }
        public string ownerMail { get; set; }
        public string confirmMsg { get; set; }
        public string deleteMsg { get; set; }

    }
}
