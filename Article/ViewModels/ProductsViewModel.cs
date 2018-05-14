using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Article.Core.Entities;

namespace Article.ViewModels
{
	public class ProductsViewModel
	{
		public string NamePersian { get; set; }
		public string Desctription { get; set; }
		public string CategoryName { get; set; }
		public decimal Price { get; set; }
		public UInt16 Count { get; set; }

		public UInt16 DeliveryCount { get; set; }
		public decimal TotalPrice { get; set; }

		public IEnumerable<Product> Products { get; set; }
		public IEnumerable<CartItem> CartItems { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}