using Article.Core.Entities;
using System.Collections.Generic;

namespace Article.Areas.Admin.ViewModels
{
	public class DashboardViewModel
	{
		public int Article { get; set; }
		public int Books { get; set; }
		public decimal Price { get; set; }
		public int Users { get; set; }
		public string UserLogin { get; set; }
		public string Role { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}