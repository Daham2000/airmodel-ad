using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class VarientOptionModel
    {
        [Key]
        public Guid varientOptionId { get; set; }
        public VarientModel varient { get; set; }
        [ForeignKey("varient")]
        public Guid varientId { get; set; }
        public int varientPrice { get; set; }
        public string varientName { get; set; }
        public string varientImage { get; set; }
        public int varientQty { get; set; }
    }
}
