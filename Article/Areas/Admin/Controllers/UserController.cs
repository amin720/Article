using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Article.Areas.Admin.Services;
using Article.Areas.Admin.ViewModels;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;

namespace Article.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("User")]
	[Authorize]
	public class UserController : Controller
    {
		private readonly IUserRepository _userRepository;
	    private readonly IRoleRepository _roleRepository;
	    private readonly UserService _users;

	    public UserController()
	    {
		    _userRepository = new UserRepository();
		    _roleRepository = new RoleRepository();
		    _users = new UserService(ModelState, _userRepository, _roleRepository);
	    }

	    // GET: Admin/User
	    [Route("")]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> Index()
		{
			var model = await _userRepository.GetAllUsersAsync();

			return View(model: model);
		}

	    // GET: Admin/User/Create
	    [Route("Create")]
	    [HttpGet]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> Create()
	    {
		    var model = new UserViewModel();
		    model.LoadUserRoles(await _roleRepository.GetAllRolesAsync());

		    return View(model: model);
	    }

	    // product: Admin/User/Create
	    [Route("Create")]
	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> Create(UserViewModel model)
	    {
		    var completed = await _users.CreateAsync(model);

		    if (completed)
		    {
			    return RedirectToAction("Index");
		    }

		    return View(model: model);
	    }

	    // GET: Admin/User/Edit/username
	    [HttpGet]
	    [Route("Edit/{username}")]
	    [Authorize(Roles = "admin, editor, author")]
	    public async Task<ActionResult> Edit(string username)
	    {
		    var currentUser = User.Identity.Name;

		    if (!User.IsInRole("admin") &&
		        !string.Equals(currentUser, username, StringComparison.CurrentCultureIgnoreCase))
		    {
			    return new HttpUnauthorizedResult();
		    }

		    var user = await _users.GetUserByNameAsync(username);

		    if (user == null)
		    {
			    return HttpNotFound();
		    }

		    return View(model: user);
	    }

	    // product: Admin/User/Edit/username
	    [HttpPost]
	    [Route("Edit/{username}")]
	    [ValidateAntiForgeryToken]
	    [Authorize(Roles = "admin, editor, author")]
	    public async Task<ActionResult> Edit(UserViewModel model, string username)
	    {
		    var currentUser = User.Identity.Name;
		    var isAdmin = User.IsInRole("admin");

		    if (!isAdmin &&
		        !string.Equals(currentUser, username, StringComparison.CurrentCultureIgnoreCase))
		    {
			    return new HttpUnauthorizedResult();
		    }

		    var userUpdated = await _users.UpdateUser(model);

		    if (userUpdated)
		    {
			    if (isAdmin)
			    {
				    return RedirectToAction("index");
			    }

			    return RedirectToAction("index", "Admin");
		    }

		    return View(model: model);
	    }

		// /admin/User/delete/product-to-delete
		[HttpGet]
	    [Route("Delete/{username}")]
	    [Authorize(Roles = "admin, editor")]
	    [AllowAnonymous]
	    public async Task<ActionResult> Delete(string username)
	    {
		    var user = await _userRepository.GetUserByNameAsync(username);

		    if (user == null)
		    {
			    return HttpNotFound();
		    }

		    return View(user);
	    }

		// product: Admin/User/Delete
		[HttpPost]
		[Route("Delete/{username}")]
		[ValidateAntiForgeryToken]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> Delete(string username, int? foo)
		{
			await _users.DeleteAsync(username);
			return RedirectToAction("Index", "User");
		}

	    private bool _isDisposed;
	    protected override void Dispose(bool disposing)
	    {

		    if (!_isDisposed)
		    {
			    _userRepository.Dispose();
			    _roleRepository.Dispose();
		    }
		    _isDisposed = true;

		    base.Dispose(disposing);
	    }
	}
}