using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Article.Core.Entities;

namespace Article.Services
{
	public static class SearchProduct
	{
		public static IEnumerable<Product> Search(string searchItem)
		{
			using (var db = new ArticleEntities())
			{
				var searched = db.Products.Where(t => t.NamePersian.Contains(searchItem)).ToList();

				return searched;
			}
		}
	}
}