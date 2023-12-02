using Microsoft.AspNetCore.Mvc;
using airmodel_ad.Models.ParamModels;

namespace airmodel_ad.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View("../Auth/SignIn-Page/SignIn");
        }

		public IActionResult ActionSignUp(SignupModel signupModel)
		{
			ViewBag.UserName = signupModel.UserName;
			ViewBag.UserEmail = signupModel.UserEmail;
			ViewBag.UserPassword = signupModel.UserPassword;
			return View("../Auth/SignIn-Page/SignIn");
		}

		public IActionResult ActionLogin(SignupModel signupModel)
		{
			ViewBag.UserEmail = signupModel.UserEmail;
			ViewBag.UserPassword = signupModel.UserPassword;
			return View("../Auth/SignIn-Page/SignIn");
		}
	}
}
