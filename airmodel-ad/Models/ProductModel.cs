using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class ProductModel
    {
        [Key]
        public Guid productId { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productImage { get; set; }
        public int productBasicPrice { get; set; }
        public bool hasVarients { get; set; }   
    }
}
