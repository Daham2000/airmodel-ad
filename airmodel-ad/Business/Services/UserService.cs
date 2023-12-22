using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using airmodel_ad.Models.ParamModels;
using Microsoft.EntityFrameworkCore;

namespace airmodel_ad.Business.Services
{
	public class UserService : IUserService
	{
		AppDbContext appDbContext;

		public UserService(AppDbContext context)
		{
			appDbContext = context;
		}
		public bool AddUser(SignupModel signupModel)
		{
			try
			{
				User user = new User();
				user.userId = Guid.NewGuid();
				user.userRole = "user";
				user.userEmail = signupModel.UserEmail;
				user.userPassword = signupModel.UserPassword;
				user.userName = signupModel.UserName;

				appDbContext.Add(user);
				CartModel cartModel = new CartModel();
				cartModel.userId = user.userId;
				cartModel.cartId = Guid.NewGuid();
				appDbContext.Add(cartModel);

				appDbContext.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool FindUserByEmail(string email, string password)
		{
			try
			{
				User? user = appDbContext.users.Where((u) => u.userEmail == email).Where((u) => u.userPassword == password).FirstOrDefault();
				if (user == null)
				{
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public User GetUserByEmail(string email)
		{
			try
			{
				User? user = appDbContext.users.Where((u) => u.userEmail == email).FirstOrDefault();
				if (user == null)
				{
					return new User();
				}
				return user;
			}
			catch (Exception ex)
			{
				return new User();
			}
		}

        public User GetUserByUid(Guid uId)
        {
            try
            {
                User? user = appDbContext.users.Where((u) => u.userId == uId).FirstOrDefault();
                if (user == null)
                {
                    return new User();
                }
                return user;
            }
            catch (Exception ex)
            {
                return new User();
            }
        }
    }
}
