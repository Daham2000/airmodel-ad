
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

        public DbSet<User> Users { get; set; }
    }
}
