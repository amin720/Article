using System.Collections.Generic;
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

		// GET: Shop
		//[Route("Shop")]
		public async Task<ActionResult> Shop(int? pageSize, string categoryName, int? pageNumber = 1)
		{
			pageSize = 9;

			pageNumber = pageNumber == null || pageNumber == 1 ? 1 : pageNumber;

			IEnumerable<Product> bestProducts = new List<Product>();
			if (string.IsNullOrEmpty(categoryName))
			{
				bestProducts = await _product.GetPageAsync((int)pageNumber, (int)pageSize);
			}
			else
			{
				bestProducts = await _product.GetPageByCategoryAsync((int)pageNumber, (int)pageSize, categoryName);
			}

			var model = new ProductsManagerViewModel
			{
				Categories = await _category.GetAllAsync(),
				BestSeller = bestProducts.Shuffle(),
			};

			var products = string.IsNullOrEmpty(categoryName) ? await _product.GetAllAsync(): await _product.GetAllByCategoryAsync(categoryName);

			ViewBag.CountTotalPages = CalucutePageNumbers((int)pageSize, products.Count());

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Search(string search)
		{
			var products = await _product.GetAllAsync();
			var searchItems = products.Where(p => p.NamePersian.Contains(search));

			return View(searchItems);
		}

		#region Method

		private int CalucutePageNumbers(int pageSize, int totalItems)
		{
			int totalPage = 0;

			if (totalItems % pageSize == 0)
			{
				totalPage = totalItems / pageSize;
			}
			else
			{
				totalPage = (int)(totalItems / pageSize) + 1;
			}

			return totalPage;
		}

		#endregion
	}
}