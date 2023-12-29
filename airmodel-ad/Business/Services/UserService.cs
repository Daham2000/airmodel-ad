using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using airmodel_ad.Models.ParamModels;
using airmodel_ad.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

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
                List<User> users = appDbContext.users.Where((u) => u.userEmail == signupModel.UserEmail).ToList();
				if(users.Count > 0)
				{
					return false;
				}
				User user = new User();
				user.userId = Guid.NewGuid();
				user.userRole = "user";
				user.userEmail = signupModel.UserEmail;

                user.userPassword = PasswordHasher.Hash(signupModel.UserPassword);
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

        public bool DeleteUser(Guid uId)
        {
            try
            {
                User? user = appDbContext.users.Where((u) => u.userId == uId).FirstOrDefault();
                appDbContext.users.Remove(user);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EditUser(Guid uId, User user)
        {
            try
            {
                Debug.WriteLine("user.userPassword");
                Debug.WriteLine(user.userPassword);
                Debug.WriteLine(user.userId);
                Debug.WriteLine(user.userEmail);
                Debug.WriteLine(user.userPassword.IsNullOrEmpty());
                List<User> users = appDbContext.users.Where((u) => u.userEmail == user.userEmail).ToList();

                if (users.Count > 0)
                {
                    return false;
                }
                if (user.userPassword.IsNullOrEmpty())
                {
                    User tempUser = appDbContext.users.Where((u) => u.userId == uId).FirstOrDefault();
                    Debug.WriteLine("tempUser.userEmail");

                    Debug.WriteLine(tempUser.userEmail);

                    user.userPassword = tempUser.userPassword;
                    Debug.WriteLine(user.userPassword);

                    appDbContext.SaveChanges();
                } else
                {
                    if(user.userPassword.Length < 50)
                    {
                        user.userPassword = PasswordHasher.Hash(user.userPassword);
                    }
                    
                    appDbContext.SaveChanges();
                }
            
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
				User? user = appDbContext.users.Where((u) => u.userEmail == email).FirstOrDefault();
                if (user == null)
				{
					return false;
				}
                Debug.WriteLine("user.userPassword");
                Debug.WriteLine(user.userPassword);
                bool isPasswordMatched = PasswordHasher.Verify(user.userPassword, password);
                if (!isPasswordMatched)
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

        public List<User> GetAllUsers()
        {
            try
            {
                List<User> users = appDbContext.users.ToList();
				return users;
            }
            catch (Exception ex)
            {
                return new List<User>();
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
