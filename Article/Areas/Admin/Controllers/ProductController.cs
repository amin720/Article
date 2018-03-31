using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Article.Areas.Admin.ViewModels;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;

namespace Article.Areas.Admin.Controllers
{
	// /admin/product

	[RouteArea("Admin")]
	[RoutePrefix("Product")]
	[Authorize]
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _usersRepository;
		private readonly ICategoryRepository _categoryRepostitory;

		public ProductController()
			: this(new ProductRepository(), new UserRepository(), new CategoryRepository())
		{
		}

		public ProductController(IProductRepository repository, IUserRepository userRepository, ICategoryRepository productCategoryRepostitory)
		{
			_productRepository = repository;
			_usersRepository = userRepository;
			_categoryRepostitory = productCategoryRepostitory;
		}

		// GET: Admin/Product
		[Route("")]
		public async Task<ActionResult> Index()
		{
			if (!User.IsInRole("author"))
			{
				return View(await _productRepository.GetAllAsync());
			}

			var user = await GetloggedInUser();
			var products = await _productRepository.GetProductsByAuthorAsync(user.Id);

			return View(products);
		}

		// /admin/Product/CreateArticle
		[HttpGet]
		[Route("CreateArticle")]
		public async Task<ActionResult> CreateArticle()
		{
			var categories = await _categoryRepostitory.GetAllAsync();
			var model = new ProductViewModel()
			{
				Categories = categories.Where(c => c.CategoryType == "article"),
			};


			return View(model: model, viewName: "Create");
		}

		// /admin/Product/CreateArticle
		[HttpGet]
		[Route("CreateBook")]
		public async Task<ActionResult> CreateBook()
		{
			var categories = await _categoryRepostitory.GetAllAsync();
			var model = new ProductViewModel()
			{
				Categories = categories.Where(c => c.CategoryType == "book"),
			};


			return View(model: model, viewName: "Create");
		}

		// /admin/Product/create
		[HttpPost]
		[Route("Create")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(ProductViewModel model, HttpPostedFileBase file, HttpPostedFileBase image)
		{
			model.Categories = await _categoryRepostitory.GetAllAsync();
			try
			{
				var product = new Product();
				var categories = await _categoryRepostitory.GetAllAsync();
				model.Categories = categories;

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var user = await GetloggedInUser();

				if (string.IsNullOrWhiteSpace(model.NamePersian)/* || string.IsNullOrWhiteSpace(model.NameEnglish)*/)
				{
					ModelState.AddModelError(string.Empty, "یک نام مناسب انتخاب کنید");
				}

				if (string.IsNullOrWhiteSpace(model.Price.ToString(CultureInfo.InvariantCulture)))
				{
					ModelState.AddModelError(string.Empty, "یک قیمت مناسب انتخاب کنید");
				}

				if (string.IsNullOrWhiteSpace(model.SKU))
				{
					ModelState.AddModelError(string.Empty, "یک کد محصول مناسب انتخاب کنید");
				}

				var category = categories.SingleOrDefault(cat => cat.Id == model.CategoryId);
				//var category = _categoryRepostitory.GetAsync(model.CategoryId);

				product.NamePersian = model.NamePersian;
				product.NameEnglish = model.NameEnglish;
				product.Price = model.Price;
				product.SKU = model.SKU;
				// Default Image
				//product.ImageUrl = model.ImageUrl;
				product.Discount = model.Discount;
				product.DescriptionEnglish = model.DescriptionEnglish;
				product.DescriptionPersian = model.DescriptionPersian;
				product.YearPublish = model.YearPublish;
				product.Source = model.Source;
				product.AuthorName = model.AuthorName;
				product.AuthorId = user.Id;
				product.Published = DateTime.Now;
				product.CategoryId = category.Id;

				var allowedExtensionsFiles = new[] {
					".pdf", ".doc", ".docx"
				};

				var allowedExtensionsImages = new[] {
					".Jpg", ".png", ".jpg", "jpeg"
				};

				if (file != null)
				{
					var fileName = Path.GetFileName(file.FileName);
					var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)
					if (allowedExtensionsFiles.Contains(ext)) //check what type of extension
					{
						string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
						string myfile = name + "_" + product.SKU + ext; //appending the name with id
						// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
						var path = Path.Combine(Server.MapPath("~/assets/files"), myfile);
						product.FileUrl = "~/assets/files/" + myfile;
						product.ImageUrl = "~/assets/images/pen.jpg";

						file.SaveAs(path);
					}
					else
					{
						ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت pdf و doc و docx انتخاب کنید");
					}

				}
				if (image != null)
				{
					var fileName = Path.GetFileName(image.FileName);
					var ext = Path.GetExtension(image.FileName); //getting the extension(ex-.jpg)
					if (allowedExtensionsImages.Contains(ext)) //check what type of extension
					{
						string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
						string myfile = name + "_" + product.SKU + ext; //appending the name with id
						// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
						var path = Path.Combine(Server.MapPath("~/assets/images"), myfile);
						product.ImageUrl = "~/assets/images/" + myfile;

						image.SaveAs(path);
					}
					else
					{
						ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت pdf و doc و docx انتخاب کنید");
					}
				}

				// TODO: update model in data store
				await _productRepository.CreateAsync(product);

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(model: model, viewName: "Create");
			}

		}

		// /admin/Product/edit/product-to-edit
		[HttpGet]
		[Route("Edit/{productId}")]
		public async Task<ActionResult> Edit(int productId)
		{

			// TODO: to retrieve the model from the data store
			var product = await _productRepository.GetByIdAsync(productId);
			var model = new ProductViewModel();

			if (product == null)
			{
				return HttpNotFound();
			}

			var category = await _categoryRepostitory.GetAsync((int) product.CategoryId);

			model.CategoryId = (int) product.CategoryId;
			model.CategoryName = true;
			model.NamePersian = product.NamePersian;
			model.NameEnglish = product.NameEnglish;
			model.Price = product.Price;
			model.SKU = product.SKU;
			// Default Image
			model.ImageUrl = product.ImageUrl;
			model.FileUrl = product.FileUrl;
			model.Discount = product.Discount;
			model.DescriptionEnglish = product.DescriptionEnglish;
			model.DescriptionPersian = product.DescriptionPersian;
			model.YearPublish = product.YearPublish;
			model.Source = product.Source;
			model.AuthorName = product.AuthorName;



			model.Categories = await _categoryRepostitory.GetAllAsync();

			if (User.IsInRole("author"))
			{
				var user = await GetloggedInUser();

				if (product.AuthorId != user.Id)
				{
					return new HttpUnauthorizedResult();
				}
			}

			return View(model: model);
		}

		// /admin/Product/edit/product-to-edit
		[HttpPost]
		[Route("Edit/{postId}")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int productId, ProductViewModel model, HttpPostedFileBase file)
		{
			model.Categories = await _categoryRepostitory.GetAllAsync();

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (User.IsInRole("author"))
			{
				var user = await GetloggedInUser();
				var product = await _productRepository.GetByIdAsync(productId);
				try
				{
					if (product.AuthorId != user.Id)
					{
						return new HttpUnauthorizedResult();
					}
				}
				catch (Exception)
				{

					throw new ArgumentException("کاربر دسترسی ندارد.");
				}

			}

			if (string.IsNullOrWhiteSpace(model.NamePersian) || string.IsNullOrWhiteSpace(model.NameEnglish))
			{
				ModelState.AddModelError("", "یک نام مناسب انتخاب کنید");
			}

			if (string.IsNullOrWhiteSpace(model.Price.ToString(CultureInfo.InvariantCulture)))
			{
				ModelState.AddModelError("", "یک قیمت مناسب انتخاب کنید");
			}

			if (string.IsNullOrWhiteSpace(model.SKU))
			{
				ModelState.AddModelError("key", "یک کد محصول مناسب انتخاب کنید");
			}

			try
			{
				var product = new Product
				{
					NamePersian = model.NamePersian,
					NameEnglish = model.NameEnglish,
					Price = model.Price,
					SKU = model.SKU,
					Discount = model.Discount,
					DescriptionEnglish = model.DescriptionEnglish,
					DescriptionPersian = model.DescriptionPersian,
					YearPublish = model.YearPublish,
					Source = model.Source,
					AuthorName = model.AuthorName,
					Published = DateTime.Now,
					CategoryId = model.CategoryId
				};

				var allowedExtensions = new[] {
					".pdf", ".doc", ".docx"
				};

				var fileName = Path.GetFileName(file.FileName);
				var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)
				if (allowedExtensions.Contains(ext)) //check what type of extension
				{
					string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
					string myfile = name + "_" + product.NameEnglish + ext; //appending the name with id
					// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
					var path = Path.Combine(Server.MapPath("~/assets/files"), myfile);
					product.FileUrl = "~/assets/files/" + myfile;
					product.ImageUrl = "~/assets/images/pen.jpg";

					file.SaveAs(path);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت pdf و doc و docx انتخاب کنید");
				}

				// Default Image
				//product.ImageUrl = model.ImageUrl;



				// TODO: update model in data store
				await _productRepository.EditAsync(productId, product);

				return RedirectToAction("Index");
			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(model);
			}
		}

		// /admin/Product/delete/product-to-delete
		[HttpGet]
		[Route("Delete/{productId}")]
		[Authorize(Roles = "admin, editor")]
		[AllowAnonymous]

		public async Task<ActionResult> Delete(int productId)
		{
			var product = await _productRepository.GetByIdAsync(productId);

			if (product == null)
			{
				return HttpNotFound();
			}

			return View(product);
		}

		// /admin/Product/delete/product-to-delete
		[HttpPost]
		[Route("Delete/{productId}")]
		[Authorize(Roles = "admin, editor")]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]

		public async Task<ActionResult> Delete(int productId, string foo)
		{

			try
			{
				using (var db = new ArticleEntities())
				{
					var cart = await db.CartItems.Where(p => p.ProductId == productId).ToListAsync();
					if (cart.Count != 0)
					{
						db.CartItems.Remove(cart.First());
					}

					await _productRepository.DeleteAsync(productId);

					return RedirectToAction("Index");
				}
			}
			catch (KeyNotFoundException e)
			{

				return HttpNotFound();
			}
		}

		#region Method

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_usersRepository.Dispose();
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}

		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _usersRepository.GetUserByNameAsync(User.Identity.Name);
		}

		private IList<Category> _categories = new List<Category>();
		private void CategoryList(Category item)
		{
			if (item.Id != null)
			{
				_categories.Add(item);

			}
		}

		#endregion
	}
}