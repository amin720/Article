using Article.Core.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Article.Areas.Admin.ViewModels
{
	public class ProductViewModel
	{
		public string NamePersian { get; set; }
		public string NameEnglish { get; set; }
		[AllowHtml]
		public string DescriptionEnglish { get; set; }
		[AllowHtml]
		public string DescriptionPersian { get; set; }
		[AllowHtml]
		public string Source { get; set; }
		public string AuthorName { get; set; }
		public string FileUrl { get; set; }
		public string ImageUrl { get; set; }
		public int CategoryId { get; set; }
		public bool CategoryName { get; set; }
		public string SKU { get; set; }
		public decimal Price { get; set; }
		public decimal? Discount { get; set; }
		public string YearPublish { get; set; }
		public string AuthorId { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}