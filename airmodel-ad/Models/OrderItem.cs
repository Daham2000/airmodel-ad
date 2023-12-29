using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class OrderItem
    {
        private Guid _oItemId;
        private OrderModel _orders;
        private Guid _oId;
        private ProductModel _products;
        private Guid _productId;
        private int _qty;
        private VarientOptionModel _varientOption;
        private Guid? _varientOptionId;

        [Key]
        public Guid oItemId
        {
            get { return _oItemId; }
            set { _oItemId = value; }
        }

        public OrderModel orders
        {
            get { return _orders; }
            set { _orders = value; }
        }

        [ForeignKey("orders")]
        public Guid oId
        {
            get { return _oId; }
            set { _oId = value; }
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

        public int qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        public VarientOptionModel varientOption
        {
            get { return _varientOption; }
            set { _varientOption = value; }
        }

        [ForeignKey("varientOption")]
        public Guid? varientOptionId
        {
            get { return _varientOptionId; }
            set { _varientOptionId = value; }
        }
    }
}
