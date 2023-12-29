using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airmodel_ad.Models
{
    public class ProductModel
    {
        private Guid _productId;
        private Category _category;
        private Guid _categoryId;
        private string _productName;
        private string _categoryName;
        private string _productDescription;
        private string _productImage;
        private int _productBasicPrice;
        private int _productQty;
        private bool _hasVarients;
        private List<VarientOptionModel> _varientOptionModels;
        private List<VarientModel> _varientModels;

        [Key]
        public Guid productId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public Category category
        {
            get { return _category; }
            set { _category = value; }
        }

        [ForeignKey("category")]
        public Guid categoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public string productName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        [NotMapped]
        public string categoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        public string productDescription
        {
            get { return _productDescription; }
            set { _productDescription = value; }
        }

        public string productImage
        {
            get { return _productImage; }
            set { _productImage = value; }
        }

        public int productBasicPrice
        {
            get { return _productBasicPrice; }
            set { _productBasicPrice = value; }
        }

        public int productQty
        {
            get { return _productQty; }
            set { _productQty = value; }
        }

        public bool hasVarients
        {
            get { return _hasVarients; }
            set { _hasVarients = value; }
        }

        [NotMapped]
        public List<VarientOptionModel> varientOptionModels
        {
            get { return _varientOptionModels; }
            set { _varientOptionModels = value; }
        }

        [NotMapped]
        public List<VarientModel> varientModels
        {
            get { return _varientModels; }
            set { _varientModels = value; }
        }
    }
}
