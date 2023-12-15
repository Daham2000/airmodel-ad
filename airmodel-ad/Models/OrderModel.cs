using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class OrderModel
    {
        [Key]
        public Guid oId { get; set; }
        public User users { get; set; }
        [ForeignKey("users")]
        public Guid userId { get; set; }
        public int total { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string county { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postCode { get; set; }
        public string phoneNumber { get; set; }
    }
}
