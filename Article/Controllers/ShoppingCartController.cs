using System;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;
using Article.Services;
using Article.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Article.Controllers
{
	[RoutePrefix("ShoppingCart")]
	[Authorize(Roles = "user,admin")]
	public class ShoppingCartController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _userRepository;

		public ShoppingCartController()
			: this(new ProductRepository(), new UserRepository())
		{

		}

		public ShoppingCartController(IProductRepository productRepository, IUserRepository userRepository)
		{
			_productRepository = productRepository;
			_userRepository = userRepository;
		}

		// GET: User/ShoppingCart
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		//[Route("AddToCart/{productId}")]
		//public async Task<ActionResult> AddToCart(ProductsViewModel model)
		public async Task<JsonResult> AddToCart(int productId)
		{
			var cart = new ShoppingCart(HttpContext);
			var user = await GetloggedInUser();


			await cart.AddAsync(productId,user.Id);

			var carts = await cart.GetCartItemsAsync(user.Id);

			var model = new ProductsManagerViewModel
			{
				CartItems = carts,
				TotalPriceItems = CalcuateCartSubtotal(carts),
				TotalPriceWithDiscount = CalcuateCartWithDiscount(carts)
			};

			//return Json(RedirectToAction("Index", "Home"));
			return Json(model, JsonRequestBehavior.AllowGet);
			//return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		//[Route("RemoveFromCart/{nameProduct}")]
		//public async Task<ActionResult> RemoveFromCart(string nameProduct)
		public async Task<JsonResult> RemoveFromCart(int productId)
		{
			var cart = new ShoppingCart(HttpContext);
			var user = await GetloggedInUser();

			await cart.RemoveAsync(productId,user.Id);
			var carts = await cart.GetCartItemsAsync(user.Id);


			var model = new ProductsManagerViewModel
			{
				CartItems = carts,
				TotalPriceItems = CalcuateCartSubtotal(carts),
				TotalPriceWithDiscount = CalcuateCartWithDiscount(carts)
			}; 


			//return Json(RedirectToAction("Dashboard", "Profile"),JsonRequestBehavior.DenyGet);
			return Json(model, JsonRequestBehavior.AllowGet);
			//return RedirectToAction("Dashboard", "Profile");

		}
		[HttpGet]
		[Route("Checkout")]
		public async Task<ActionResult> Checkout()

		{
			var user = await GetloggedInUser();
			var cart = new ShoppingCart(HttpContext);
			var carts = await cart.GetCartItemsAsync(user.Id);


			var model = new CheckoutViewModel
			{
				FirstName = user.DisplayName,
				Email = user.Email,
				CardNumber = "4111111111111111",
				Phone = user.PhoneNumber,
				Cvv = "124",
				Month = "07",
				Year = "2020",
				CartItems = carts
			};

			return View(model: model, viewName: "Checkout");
		}
		[HttpPost]
		[Route("Checkout")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Checkout(CheckoutViewModel model)
		{
			var user = await GetloggedInUser();
			
			model.Email = user.Email;
			model.CardNumber = "4111111111111111";
			model.Cvv = "124";
			model.Month = "07";
			model.Year = "2020";


			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var cart = new ShoppingCart(HttpContext);

			var result = await cart.CheckoutAsync(model);

			//---- start

			result.Succeeded = true;
			result.TransactionId = RandomString(10);

			//---- end

			if (result.Succeeded)
			{
				TempData["transactionId"] = result.TransactionId;
				return RedirectToAction("Complete");
			}
			ModelState.AddModelError(string.Empty, result.Message);

			return View(model);
		}

		[HttpGet]
		[Route("Complete")]
		public async Task<ActionResult> Complete()
		{
			var cart = new ShoppingCart(HttpContext);
			var user = await GetloggedInUser();

			var result = await cart.GetCartItemsAsync(user.Id);

			ViewBag.TransactionId = (string)TempData["transactionId"];

			var model = new CheckoutViewModel()
			{
				//FirstName = user.FirstName,
				//LastName = user.LastName,
				//Address = user.Address,
				//PostalCode = user.PostalCode,
				Phone = user.PhoneNumber,
				CartItems = result,
				Subtotal = CalcuateCartSubtotal(result),
				Total = CalcuateCartWithDiscount(result)
			};

			return View(model: model, viewName: "Complete");
		}

		private static decimal CalcuateCartSubtotal(IEnumerable<CartItem> items)
		{
			var total = 0m;

			foreach (var item in items)
			{
				total += (item.Product.Price * item.Count);
			}

			return total;
		}
		private static decimal CalcuateCartWithDiscount(IEnumerable<CartItem> items)
		{
			var total = 0m;

			foreach (var item in items)
			{
				if (item.Product.Discount == null)
				{
					item.Product.Discount = 0;
				}

				total += ((item.Product.Price - (decimal)item.Product.Discount) * item.Count);
			}

			return total;
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


		private static readonly Random _random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[_random.Next(s.Length)]).ToArray());
		}

		#endregion
	}
}