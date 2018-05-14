using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;
using Article.Services;
using Article.ViewModels;

namespace Article.Controllers
{
	[AllowAnonymous]
	[RoutePrefix("Product")]
	public class ProductsController : Controller
	{
		private readonly ICategoryRepository _category;
		private readonly IProductRepository _product;
		private readonly ICommentRepository _comment;
		private readonly IUserRepository _user;

		public ProductsController()
			: this(new CategoryRepository(), new ProductRepository(), new UserRepository(), new CommentRepository())
		{

		}

		public ProductsController(ICategoryRepository category, IProductRepository product, IUserRepository user, ICommentRepository comment)
		{
			_category = category;
			_product = product;
			_comment = comment;
			_user = user;
		}


		// GET: Single
		[Route("/Single/{id}")]
		public async Task<ActionResult> Single(int Id)
		{
			var product = await _product.GetByIdAsync(Id);
			var user = await GetloggedInUser();

			IEnumerable<CartItem> carts = null;
			var cart = new ShoppingCart(HttpContext);
			if (user == null)
			{
				carts = await cart.GetCartItemsAsync(String.Empty);
			}
			else
			{
				carts = await cart.GetCartItemsAsync(user.Id);
			}

			var model = new ProductDetailViewModel
			{
				ProductId = product.Id,
				PersianTitle = product.NamePersian,
				EnglishTitle = product.NameEnglish,
				PersianDescription = product.DescriptionPersian,
				EnglishDescription = product.DescriptionEnglish,
				Price = product.Price,
				Discount = product.Discount,
				SKU = product.SKU,
				Source = product.Source,
				YearPublish = product.YearPublish,
				ImageUrl = product.ImageUrl,
				AuthorName = product.AuthorName,
				RelatedProduct = await _product.GetPageByCategoryAsync(1, 8, (int) product.CategoryId),
				Comments = await _comment.GetAllByProductId(Id),
				CartItems = carts
			};

			return View(model: model, viewName: "Single");
		}

		[HttpPost]
		public async Task<ActionResult> Comment(ProductDetailViewModel model)
		{
			var comment = new Comment();
			try
			{
				if (!ModelState.IsValid)
				{
					return RedirectToAction("Single", new { id = model.ProductId });

				}

				comment.Title = model.CommentTitle;
				comment.Desciption = model.CommentDescription;
				comment.Email = model.Email;
				comment.Name = model.PersonName;
				comment.Price = model.SurveyPrice;
				comment.Vakue = model.SurveyValue;
				comment.ProductId = model.ProductId;
				

				await _comment.CreateAsync(comment);
				ViewBag.CommentRelatedProduct = await _comment.GetAllByProductId((int)model.ProductId);
				return RedirectToAction("Single",new {id = model.ProductId});

			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return RedirectToAction("Single", new { id = model.ProductId });

			}
		}

		// GET: Download
		public async Task<FileResult> Download(int? Id)
		{
			var id = Id == 0 || Id == null ? (int)TempData["Id"] : Id;
			var model = await _product.GetByIdAsync((int)id);

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

		#region Method

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_user.Dispose();
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}
		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _user.GetUserByNameAsync(User.Identity.Name);
		}

		#endregion
	}
}