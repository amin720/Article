using System.ComponentModel.DataAnnotations;

namespace Article.Areas.Admin.ViewModels
{
	public class LoginViewModel
	{
		[Display(Name = "Username")]
		public string UserName { get; set; }

		public string Password { get; set; }
		
		public string RecoveryPassword { get; set; }

		public string Email { get; set; }

		[Display(Name = "Remmeber Me")]
		public bool Remmeberme { get; set; }
	}
}