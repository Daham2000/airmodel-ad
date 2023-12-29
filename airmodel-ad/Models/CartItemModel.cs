using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class CartItemModel
    {
        private Guid _cartItemId;
        private CartModel _cart;
        private Guid _cartId;
        private ProductModel _products;
        private Guid _productId;
        private int _qty;
        private VarientOptionModel? _varientOption;
        private Guid? _varientOptionId;

        [Key]
        public Guid cartItemId
        {
            get { return _cartItemId; }
            set { _cartItemId = value; }
        }

        public CartModel cart
        {
            get { return _cart; }
            set { _cart = value; }
        }

        [ForeignKey("cart")]
        public Guid cartId
        {
            get { return _cartId; }
            set { _cartId = value; }
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

        public VarientOptionModel? varientOption
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
