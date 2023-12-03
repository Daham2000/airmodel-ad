using Microsoft.AspNetCore.Mvc;
using airmodel_ad.Models.ParamModels;
using airmodel_ad.Business.Interface;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
			ClaimsPrincipal claimsPrincipal = HttpContext.User;
			if(claimsPrincipal.Identity.IsAuthenticated)
			{
                return RedirectToAction("Index", "Home");
            }
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

		[HttpPost]
		public async Task<IActionResult> ActionLogin(SignupModel signupModel)
		{
			ViewBag.UserEmail = signupModel.UserEmail;
			ViewBag.UserPassword = signupModel.UserPassword;
			bool result = userService.FindUserByEmail(signupModel.UserEmail, signupModel.UserPassword);
			if (result)
			{
				List<Claim> claims = new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, signupModel.UserEmail),
					new Claim("OtherProperties", "User")
				};

				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, 
					CookieAuthenticationDefaults.AuthenticationScheme);

				AuthenticationProperties authenticationProperties = new AuthenticationProperties()
				{
					AllowRefresh = true,
					IsPersistent = true
				};

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);

				return RedirectToAction("Index", "Home");
			}
            ViewBag.ValidateMessage = "User not found";
            return View("../Auth/SignIn-Page/SignIn");
		}
	}
}
