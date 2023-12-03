using Microsoft.AspNetCore.Mvc;
using airmodel_ad.Models.ParamModels;
using airmodel_ad.Business.Interface;

namespace airmodel_ad.Controllers
{
    public class AuthController : Controller
    {
		private readonly IUserService userService;

		public AuthController(IUserService userService)
		{
			this.userService = userService;
		}

		public IActionResult Index()
        {
            return View("../Auth/SignIn-Page/SignIn");
        }

		public IActionResult ActionSignUp(SignupModel signupModel)
		{
			ViewBag.UserName = signupModel.UserName;
			ViewBag.UserEmail = signupModel.UserEmail;
			ViewBag.UserPassword = signupModel.UserPassword;
			bool result = userService.AddUser(signupModel);
			if(result)
			{
				return View("../Home/Index");
			}
			return View("../Auth/SignIn-Page/SignIn");
		}

		public IActionResult ActionLogin(SignupModel signupModel)
		{
			ViewBag.UserEmail = signupModel.UserEmail;
			ViewBag.UserPassword = signupModel.UserPassword;
			bool result = userService.FindUserByEmail(signupModel.UserEmail, signupModel.UserPassword);
			if (result)
			{
				return View("../Home/HomeView");
			}
			return View("../Auth/SignIn-Page/SignIn");
		}
	}
}
