using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class CartModel
    {
        [Key]
        public Guid cartId { get; set; }
        public User users { get; set; }
        [ForeignKey("users")]
        public Guid userId { get; set; }

    }
}
