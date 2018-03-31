using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Article.Areas.Admin.ViewModels;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;
using Microsoft.Owin.Security;

namespace Article.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	[RoutePrefix("")]
	[Authorize]
	public class AdminController : Controller
    {
	    private readonly IUserRepository _userRepository;
	    private readonly ICategoryRepository _categoryRepostitory;

	    public AdminController()
		    : this(new UserRepository(), new CategoryRepository())
	    {
	    }

	    public AdminController(IUserRepository userRepository, ICategoryRepository categoryRepostitory)
	    {
		    _userRepository = userRepository;
		    _categoryRepostitory = categoryRepostitory;
	    }

		// GET: Admin/Admin
	    [Route("")]
		public async Task<ActionResult> Index()
	    {
		    var users = await _userRepository.GetAllUsersAsync();
		    var categories = await _categoryRepostitory.GetAllAsync();
		    int articleCount = categories.Count(c => c.CategoryType == "article");
		    int bookCount = categories.Count(c => c.CategoryType == "book");

			var model = new DashboardViewModel()
			{
				UserLogin = User.Identity.Name,
				Users = users.Select(u => u.Roles.Where(r => r.RoleId == "54ec7407-2468-4579-82d9-99e701f5c761")).Count(),
				Price = 0,
				Article = articleCount,
				Books = bookCount,
				Categories = await _categoryRepostitory.GetAllAsync()

			};
		    if (User.IsInRole("admin"))
		    {
			    model.Role = "Admin";
		    }
		    else if (User.IsInRole("editor"))
		    {
			    model.Role = "Editor";
		    }
		    else if (User.IsInRole("author"))
		    {
			    model.Role = "Author";
		    }


			return View(model);
        }

	    // GET: Admin/Admin/Login
	    [HttpGet]
	    [Route("login")]
	    [AllowAnonymous]
	    public async Task<ActionResult> Login()
	    {
		    return View();
	    }

	    // product: Admin/Admin/Login
	    [HttpPost]
	    [Route("login")]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
	    public async Task<ActionResult> Login(LoginViewModel model)
	    {
		    try
		    {
			    var user = await _userRepository.GetLoginUserAsync(model.UserName, model.Password);

			    if (user == null)
			    {
				    //ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
				    ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود نیست.");
			    }

			    var authManager = HttpContext.GetOwinContext().Authentication;

			    var userIdentity = await _userRepository.CreateIdentityAsync(user);

			    authManager.SignIn(new AuthenticationProperties() { IsPersistent = model.Remmeberme }, userIdentity);


			    return RedirectToAction("Index","Admin");
			}
		    catch (Exception e)
		    {
				ModelState.AddModelError(string.Empty, e.Message);
			    return View(viewName:"Login");
		    }
	    }


	    // GET: Admin/Admin/Logout
	    [HttpGet]
	    [Route("logout")]
	    public async Task<ActionResult> Logout()
	    {
		    var authManager = HttpContext.GetOwinContext().Authentication;

		    authManager.SignOut();

		    return RedirectToAction("Index");
	    }

	    // GET: Admin/Admin/ForgotPassword
	    [HttpGet]
	    [Route("forgotpassword")]
	    [AllowAnonymous]
	    public async Task<ActionResult> Forgotpassword()
	    {
		    return View(viewName: "Forgotpassword");
	    }


	    // product: Admin/ForgotPassword
	    [HttpPost]
	    [Route("forgotpassword")]
	    [AllowAnonymous]
	    public async Task<ActionResult> Forgotpassword(LoginViewModel model)
	    {
		    try
		    {
			    if (!ModelState.IsValid)
			    {
				    return HttpNotFound();
			    }

			    var users = await _userRepository.GetAllUsersAsync();

			    var user = users.SingleOrDefault(u => u.UserName == model.UserName || u.Email == model.Email);

			    if (user == null)
			    {
				    ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
			    }

			    model.RecoveryPassword = user.PasswordHash;

			    return RedirectToAction("RecoveryPassword", model);

			    //return PartialView(model: userPassword, viewName: "Password");
		    }
		    catch (Exception e)
		    {
			    ModelState.AddModelError("keys", e);
			    return RedirectToAction("Login");
		    }

	    }

	    [HttpGet]
	    [Route("RecoveryPassword")]
	    [AllowAnonymous]
	    public ActionResult RecoveryPassword(LoginViewModel model)
	    {
		    //var model = new LoginViewModel();

		    return View(viewName: "RecoveryPassword", model: model);
	    }

	    [AllowAnonymous]
	    public async Task<PartialViewResult> AdminMenu()
	    {
		    var items = new List<AdminMenuItem>();

		    if (User.Identity.IsAuthenticated)
		    {
			    items.Add(new AdminMenuItem
			    {
				    Text = "داشبرد",
				    Action = "index",
				    RouteInfo = new { controller = "admin", area = "admin" }
			    });

			    if (User.IsInRole("admin"))
			    {
				    items.Add(new AdminMenuItem
				    {
					    Text = "کاربران",
					    Action = "index",
					    RouteInfo = new { controller = "user", area = "admin" }
				    });

					items.Add(new AdminMenuItem()
					{
						Text = "محصولات",
						Action = "index",
						RouteInfo = new { controller = "product", area = "admin" }
					});
			    }
			    else
			    {
				    items.Add(new AdminMenuItem
				    {
					    Text = "تنظیمات پروفایل",
					    Action = "edit",
					    RouteInfo = new { controller = "user", area = "admin", username = User.Identity.Name }
				    });
			    }

			    //if (!User.IsInRole("author"))
			    //{
				   // items.Add(new AdminMenuItem
				   // {
					  //  Text = "Tags",
					  //  Action = "index",
					  //  RouteInfo = new { controller = "tag", area = "admin" }
				   // });
			    //}

			    //items.Add(new AdminMenuItem
			    //{
				   // Text = "Posts",
				   // Action = "index",
				   // RouteInfo = new { controller = "product", area = "admin" }
			    //});

			    items.Add(new AdminMenuItem
			    {
				    Text = "دسته بندی محصولات",
				    Action = "index",
				    RouteInfo = new { controller = "category", area = "admin" }
			    });
		    }

		    return PartialView(items);
	    }

	    [AllowAnonymous]
	    public async Task<PartialViewResult> AuthenticationLink()
	    {
		    var item = new AdminMenuItem
		    {
			    RouteInfo = new { controller = "admin", area = "admin" }
		    };

		    if (User.Identity.IsAuthenticated)
		    {
			    item.Text = "خروج";
			    item.Action = "logout";
		    }
		    else
		    {
			    item.Text = "وارد شدن";
			    item.Action = "login";
		    }

		    return PartialView("_menuLink", item);
	    }

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
	}
}