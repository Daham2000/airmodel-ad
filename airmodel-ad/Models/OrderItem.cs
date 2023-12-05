using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class OrderItem
    {
        [Key]
        public Guid oItemId { get; set; }
        public OrderModel orders { get; set; }
        [ForeignKey("orders")]
        public Guid oId { get; set; }
        public ProductModel products { get; set; }
        [ForeignKey("products")]
        public Guid productId { get; set; }
        public int qty { get; set; }
        public VarientOptionModel varientOption { get; set; }
        [ForeignKey("varientOption")]
        public Guid varientOptionId { get; set; }
    }
}
