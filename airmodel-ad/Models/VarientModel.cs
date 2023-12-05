using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class VarientModel
    {
        [Key]
        public Guid varientId { get; set; }
        public ProductModel products { get; set; }
        [ForeignKey("products")]
        public Guid productId { get; set; }
        public string productName { get; set; } 
    }
}
