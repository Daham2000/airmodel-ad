using airmodel_ad.Models;
using airmodel_ad.Utils;

namespace airmodel_ad.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (context != null)
                {
                    context.Database.EnsureCreated();

                    // User Seeding
                    if (!context.users.Any())
                    {
                        context.users.AddRange(new List<User>()
                        {
                            new User(){
                                userId = Guid.NewGuid(),
                                userName = "admin",
                                userEmail = "admin@yopmail.com",
                                userPassword = PasswordHasher.Hash("admin"),
                                userRole = "admin"
                            }
                        });
                    }


                    context.SaveChanges();
                }
            }
        }
    }
}
