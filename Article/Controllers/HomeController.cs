using System.Linq;
using System.Threading.Tasks;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;
using System.Web.Mvc;
using Article.Core.Entities;
using Article.ViewModels;
using static Article.Services.Extensions;

namespace Article.Controllers
{
	[AllowAnonymous]
	[RoutePrefix("")]
	public class HomeController : Controller
	{
		private readonly ICategoryRepository _category;
		private readonly IProductRepository _product;
		private readonly IUserRepository _user;

		public HomeController()
			: this(new CategoryRepository(), new ProductRepository(), new UserRepository())
		{

		}

		public HomeController(ICategoryRepository category, IProductRepository product, IUserRepository user)
		{
			_category = category;
			_product = product;
			_user = user;
		}

		// GET: Home
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var bestProducts = await _product.GetPageAsync(1, 8);
			var freeProducts = await _product.GetPageAsync(1, 8);
			var artistProducts = await _product.GetPageByCategoryAsync(1, 5, "گرافیک");
			var engeenierProducts = await _product.GetPageByCategoryAsync(1, 5, "کامپیوتر");

			var model = new ProductsManagerViewModel
			{
				BestSeller = bestProducts.Shuffle(),
				FreeProducts = freeProducts.Where(p => p.Price == 0).Shuffle(),
				ArtisticProducts = artistProducts.Shuffle(),
				EngeenierProducts = engeenierProducts.Shuffle()
			};

			return View(model);
		}

		// GET: About
		[Route("About")]
		public ActionResult About()
		{
			return View();
		}

		// GET: Faq
		[Route("Faq")]
		public ActionResult Faq()
		{
			return View();
		}

		// GET: Grid
		[Route("Grid")]
		public ActionResult Grid()
		{
			return View();
		}
	}
}