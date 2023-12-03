using airmodel_ad.Models.ParamModels;

namespace airmodel_ad.Business.Interface
{
	public interface IUserService
	{
		public bool AddUser(SignupModel signupModel);
		public bool FindUserByEmail(string email, string password);
	}
}
