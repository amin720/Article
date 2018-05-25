using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Article.Areas.Admin.ViewModels
{
	public class UsersInRole
	{
		public string FullName { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
	}
}