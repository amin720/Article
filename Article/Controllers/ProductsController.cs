using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;

namespace Article.Controllers
{
	[AllowAnonymous]
	[RoutePrefix("Product")]
	public class ProductsController : Controller
	{
		private readonly ICategoryRepository _category;
		private readonly IProductRepository _product;
		private readonly IUserRepository _user;

		public ProductsController()
			: this(new CategoryRepository(), new ProductRepository(), new UserRepository())
		{

		}

		public ProductsController(ICategoryRepository category, IProductRepository product, IUserRepository user)
		{
			_category = category;
			_product = product;
			_user = user;
		}


		// GET: Single
		[Route("/Single/{id}")]
		public async Task<ActionResult> Single(int Id)
		{
			var model = await _product.GetByIdAsync(Id);
			ViewBag.RelatedProducts = await _product.GetPageByCategoryAsync(1, 8, (int)model.CategoryId);

			return View(model: model, viewName: "Single");
		}

		// GET: Download
		public async Task<FileResult> Download(int? Id)
		{
			var id = Id == 0 || Id == null ? (int)TempData["Id"] : Id;
			var model = await _product.GetByIdAsync((int) id);

			byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(model.FileUrl));
			string fileName = model.NameEnglish;

			string contentType = string.Empty;

			if (model.FileUrl.Contains(".pdf"))
			{
				contentType = "application/pdf";
			}

			else if (model.FileUrl.Contains(".docx"))
			{
				contentType = "application/docx";
			}
			else
			{
				contentType = "application/doc";
			}

			//return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
			//return File(fileBytes, contentType, fileName);
			return new FilePathResult(Server.MapPath(model.FileUrl), contentType);

		}

		// GET: Buy
		[Route("/Buy/{id}")]
		public async Task<ActionResult> Buy(int Id)
		{
			var model = await _product.GetByIdAsync(Id);
			if (model.Price == 0 || model.Discount == 0)
			{
				TempData["Id"] = model.Id;
				return RedirectToAction("Download", "Products");
			}

			return View(model: model);
		}

		
	}
}