using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class Category
    {
        [Key]
        public Guid categoryId { get; set; }
        public string categoryName { get; set; }

    }
}
