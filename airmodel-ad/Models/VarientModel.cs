using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class VarientModel
    {
        private Guid _varientId;
        private ProductModel _products;
        private Guid _productId;
        private string _productName;

        [Key]
        public Guid varientId
        {
            get { return _varientId; }
            set { _varientId = value; }
        }

        public ProductModel products
        {
            get { return _products; }
            set { _products = value; }
        }

        [ForeignKey("products")]
        public Guid productId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string productName
        {
            get { return _productName; }
            set { _productName = value; }
        }
    }
}
