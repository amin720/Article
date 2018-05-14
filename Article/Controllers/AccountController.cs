using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;
using Article.ViewModels;
using Microsoft.Owin.Security;

namespace Article.Controllers
{
	[RoutePrefix("Profile")]
    public class AccountController : Controller
    {
		private readonly IUserRepository _userRepository;

	    public AccountController()
		    : this(new UserRepository())
	    {

	    }

	    public AccountController(IUserRepository userRepository)
	    {
		    _userRepository = userRepository;
	    }



		// GET: Account
		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(AccountViewModel model)
		{
			var user = await _userRepository.GetLoginUserAsync(model.Username, model.Password);

			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود نیست.");
				return View(viewName: "Login");
			}

			var authManager = HttpContext.GetOwinContext().Authentication;

			var userIdentity = await _userRepository.CreateIdentityAsync(user);

			authManager.SignIn(new AuthenticationProperties(), userIdentity);


			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Route("logout")]
		public async Task<ActionResult> Logout()
		{
			var authManager = HttpContext.GetOwinContext().Authentication;

			authManager.SignOut();

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(AccountViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(viewName: "Login");
			}

			var users = await _userRepository.GetAllUsersAsync();
			var user = users.SingleOrDefault(u => u.UserName == model.Username || u.Email == model.Email);

			if (user != null)
			{
				ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود است.");

				//ModelState.AddModelError(string.Empty, "The user with supplied credentials does exist.");
			}

			//var completed = await _users.CreateAsync(model);

			//   if (completed)
			//   {
			//    return RedirectToAction("Login", "Profile");
			//   }
			var newUser = new UserIdentity()
			{
				DisplayName = model.FullName,
				Email = model.Email,
				UserName = model.Username,
				PhoneNumber = model.Mobile
			};

			await _userRepository.CreateAsync(newUser, model.Password);

			await _userRepository.AddUserToRoleAsync(newUser, "user");

			return RedirectToAction("Login", "Account");
		}


		#region Method

		private bool _isDisposed;
	    protected override void Dispose(bool disposing)
	    {
		    if (!_isDisposed)
		    {
			    _userRepository.Dispose();
		    }

		    _isDisposed = true;
		    base.Dispose(disposing);
	    }

	    private async Task<UserIdentity> GetloggedInUser()
	    {
		    return await _userRepository.GetUserByNameAsync(User.Identity.Name);
	    }

	    #endregion

	}
}