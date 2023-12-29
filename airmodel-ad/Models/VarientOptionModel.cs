using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class VarientOptionModel
    {
        private Guid _varientOptionId;
        private VarientModel _varient;
        private Guid _varientId;
        private int _varientPrice;
        private string _varientName;
        private string _varientImage;
        private int _varientQty;

        [Key]
        public Guid varientOptionId
        {
            get { return _varientOptionId; }
            set { _varientOptionId = value; }
        }

        public VarientModel varient
        {
            get { return _varient; }
            set { _varient = value; }
        }

        [ForeignKey("varient")]
        public Guid varientId
        {
            get { return _varientId; }
            set { _varientId = value; }
        }

        public int varientPrice
        {
            get { return _varientPrice; }
            set { _varientPrice = value; }
        }

        public string varientName
        {
            get { return _varientName; }
            set { _varientName = value; }
        }

        public string varientImage
        {
            get { return _varientImage; }
            set { _varientImage = value; }
        }

        public int varientQty
        {
            get { return _varientQty; }
            set { _varientQty = value; }
        }
    }
}
