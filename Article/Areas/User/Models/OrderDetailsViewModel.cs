using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Article.Areas.User.Models
{
	public class OrderDetailsViewModel
	{
		public string ProductName { get; set; }
		public int Qty { get; set; }
		public decimal UnitPrice { get; set; }
		public int NumberOrder { get; set; }
	}
}