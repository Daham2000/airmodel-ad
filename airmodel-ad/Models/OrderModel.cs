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
    }
}
