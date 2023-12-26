using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface ICategoryService
    {
        public List<Category> GetAllCategories();
        public bool AddCategory(Category item);
        public bool RemoveCategory(Category item);
        public bool EditCategory(string name, Guid categoryId);
    }
}
