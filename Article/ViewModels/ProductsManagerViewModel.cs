using Article.Core.Entities;
using System.Collections.Generic;

namespace Article.ViewModels
{
	public class ProductsManagerViewModel
	{
		public decimal TotalPriceItems { get; set; }
		public decimal TotalPriceWithDiscount { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Product> BestSeller { get; set; }
		public IEnumerable<Product> FreeProducts { get; set; }
		public IEnumerable<Product> BookProducts { get; set; }
		public IEnumerable<Product> EngeenierProducts { get; set; }
		public IEnumerable<Product> ArtisticProducts { get; set; }
		public IEnumerable<CartItem> CartItems { get; set; }
	}
}