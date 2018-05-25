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


namespace Article.Areas.User.Controllers
{
	[RouteArea("user")]

	public class UserProfileController : Controller
    {
	    private readonly IUserRepository _userRepository;
	    private readonly ICategoryRepository _categoryRepostitory;

	    public UserProfileController()
		    : this(new UserRepository(), new CategoryRepository())
	    {
	    }
	    public UserProfileController(IUserRepository userRepository, ICategoryRepository categoryRepostitory)
	    {
		    _userRepository = userRepository;
		    _categoryRepostitory = categoryRepostitory;
	    }



	    // GET: User/User
	    [Route("")]
	    [AllowAnonymous]

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


		    return View(model:model,viewName:"Index");
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