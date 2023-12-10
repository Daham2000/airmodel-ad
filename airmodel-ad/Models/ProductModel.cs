using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class ProductModel
    {
        [Key]
        public Guid productId { get; set; }
        public Category category { get; set; }
        [ForeignKey("category")]
        public Guid categoryId { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productImage { get; set; }
        public int productBasicPrice { get; set; }
        public int productQty { get; set; }
        public bool hasVarients { get; set; }
        [NotMapped]
        public List<VarientOptionModel> varientOptionModels { get; set; }
        [NotMapped]
        public List<VarientModel> varientModels { get; set; }
    }
}
