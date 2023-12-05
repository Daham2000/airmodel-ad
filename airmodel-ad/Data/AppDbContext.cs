
using Microsoft.EntityFrameworkCore;
using airmodel_ad.Models;

namespace airmodel_ad.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Category> category { get; set; }

        public DbSet<ProductModel> products { get; set; }
        public DbSet<VarientModel> varient { get; set; }
        public DbSet<VarientOptionModel> varientOption { get; set; }
        public DbSet<OrderModel> orders { get; set; }
        public DbSet<OrderItem> orderItem { get; set; }
        public DbSet<CartModel> carts { get; set; }
        public DbSet<CartItemModel> cartItems { get; set; }
    }
}
