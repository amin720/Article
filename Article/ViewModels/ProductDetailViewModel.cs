using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Article.Core.Entities;

namespace Article.ViewModels
{
	public class ProductDetailViewModel
	{
		public int ProductId { get; set; }
		public string PersianTitle { get; set; }
		public string EnglishTitle { get; set; }
		public decimal Price { get; set; }
		public decimal? Discount { get; set; }
		public string SKU { get; set; }
		public string AuthorName { get; set; }
		public string YearPublish { get; set; }
		public string Source { get; set; }
		public string PersianDescription { get; set; }
		public string EnglishDescription { get; set; }
		public string ImageUrl { get; set; }
		

		public string CommentTitle { get; set; }
		public string PersonName { get; set; }
		public string Email { get; set; }
		public string CommentDescription { get; set; }
		public int SurveyValue { get; set; }
		public int SurveyPrice { get; set; }

		public IEnumerable<Product> RelatedProduct { get; set; }
		public IEnumerable<Comment> Comments { get; set; }
	}
}