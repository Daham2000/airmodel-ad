using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using System.Diagnostics;

namespace airmodel_ad.Business.Services
{
    public class CategoryService : ICategoryService
    {
        AppDbContext appDbContext;
        public CategoryService(AppDbContext context)
        {
            appDbContext = context;
        }
        public bool AddCategory(Category item)
        {
            try
            {
                appDbContext.Add(item);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EditCategory(string name, Guid categoryId)
        {
            try
            {
          
                Category category = appDbContext.category.Where((c)=> c.categoryId == categoryId).FirstOrDefault(); 
                category.categoryName = name;
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = appDbContext.category.ToList();
                return categories;
            }
            catch (Exception ex)
            {
                return categories;
            }
        }

        public bool RemoveCategory(Category item)
        {
            try
            {
                appDbContext.category.Remove(item);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
