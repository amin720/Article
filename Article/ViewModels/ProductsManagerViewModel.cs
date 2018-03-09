using Article.Core.Entities;
using System.Collections.Generic;

namespace Article.ViewModels
{
	public class ProductsManagerViewModel
	{
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Product> BestSeller { get; set; }
		public IEnumerable<Product> FreeProducts { get; set; }
		public IEnumerable<Product> EngeenierProducts { get; set; }
		public IEnumerable<Product> ArtisticProducts { get; set; }
	}
}