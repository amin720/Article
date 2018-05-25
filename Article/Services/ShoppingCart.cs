using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Article.Core.Entities;
using Article.ViewModels;

namespace Article.Services
{
	public class ShoppingCart
	{
		private readonly ArticleEntities _context;
		private readonly string _cartId;

		public ShoppingCart(HttpContextBase context)
			: this(context, new ArticleEntities())
		{
		}

		public ShoppingCart(HttpContextBase httpContext, ArticleEntities context)
		{
			_context = context;
			_cartId = GetCartId(httpContext);
		}

		public async Task AddAsync(int productId,string userId)
		{
			var product = await _context.Products
												.SingleOrDefaultAsync(p => p.Id == productId);

			if (product == null)
			{
				// TODO: throw exception or do something
				return;
			}

			//var cartItem = await _context.CartItems
			//										.SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId);
			var cartItem = await _context.CartItems
													.SingleOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

			if (cartItem != null)
			{
				cartItem.Count++;
			}
			else
			{
				cartItem = new CartItem
				{
					ProductId = productId,
					CartId = _cartId,
					UserId = userId,
					Count = 1,
					DateCreated = DateTime.Now
				};

				_context.CartItems.Add(cartItem);
			}

			//_context.SaveChanges();
			await _context.SaveChangesAsync();
		}

		public async Task<int> RemoveAsync(int productId, string userId)
		{
			//var cartItem = await _context.CartItems
			//	.SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId);

			var cartItem = await _context.CartItems
				.SingleOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

			var itemCount = 0;

			if (cartItem == null)
			{
				return itemCount;
			}

			if (cartItem.Count > 1)
			{
				cartItem.Count--;
				itemCount = cartItem.Count;
			}
			else
			{
				_context.CartItems.Remove(cartItem);
			}

			 await _context.SaveChangesAsync();

			return itemCount;
		}

		public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
		{
			//return await _context.CartItems.Include("Product")
			//							   .Where(c => c.CartId == _cartId )
			//							   .ToArrayAsync();
			return await _context.CartItems.Include("Product")
								.Where(c => c.UserId == userId)
								.ToArrayAsync();
		}

		public async Task<PaymentResult> CheckoutAsync(CheckoutViewModel model)
		{
			var items = await GetCartItemsAsync(model.UserId);
			var order = new Order()
			{
				FullName = model.FullName,
				Phone = model.Phone,
				Email = model.Email,
				OrderDate = DateTime.Now
			};

			foreach (var item in items)
			{
				var detail = new OrderDetail()
				{
					ProductId = item.ProductId,
					UnitPrice = item.Product.Price,
					Quantity = item.Count,
					OrderId = 1,
				};

				order.Total += (item.Product.Price * item.Count);

				order.OrderDetails.Add(detail);
			}

			model.Total = order.Total;

			//var gateway = new PaymentGateway();
			//var result = gateway.ProcessPayment(model);

			var result = new PaymentResult(RandomString(10), true, "success");
			

			if (result.Succeeded)
			{
				order.TransactionId = result.TransactionId;
				_context.Orders.Add(order);
				_context.CartItems.RemoveRange(items);
				await _context.SaveChangesAsync();
				//_context.SaveChanges();
			}

			return result;
		}

		private string GetCartId(HttpContextBase http)
		{
			var cookie = http.Request.Cookies.Get("ShoppingCart");
			var cartId = string.Empty;

			if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
			{
				cookie = new HttpCookie("ShoppingCart");
				cartId = Guid.NewGuid().ToString();

				cookie.Value = cartId;
				cookie.Expires = DateTime.Now.AddDays(7);

				http.Response.Cookies.Add(cookie);
			}
			else
			{
				cartId = cookie.Value;
			}

			return cartId;
		}



		private static readonly Random _random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[_random.Next(s.Length)]).ToArray());
		}
	}
}