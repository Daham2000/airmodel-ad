using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class CartItemModel
    {
        [Key]
        public Guid cartItemId { get; set; }
        public CartModel cart { get; set; }
        [ForeignKey("cart")]
        public Guid cartId { get; set; }
        public ProductModel products { get; set; }
        [ForeignKey("products")]
        public Guid productId { get; set; }
        public int qty { get; set; }
        public VarientOptionModel varientOption { get; set; }
        [ForeignKey("varientOption")]
        public Guid varientOptionId { get; set; }
    }
}
