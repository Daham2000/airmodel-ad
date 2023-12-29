using System.ComponentModel.DataAnnotations;

namespace airmodel_ad.Models
{
    public class Category
    {
        private Guid _categoryId;
        private string _categoryName;

        [Key]
        public Guid categoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public string categoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }
    }
}
