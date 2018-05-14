using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Article.ViewModels
{
	public class AccountViewModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		[MinLength(6, ErrorMessage = "حداقل رمز عبور 6 کاراکتر")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "رمز عبور شما بایستی حروف a-z , A-z , @")]
		//data-val-regex-pattern="([a-zA-Z0-9&#32;.&amp;amp;&amp;#39;-]+)"      <-- MVC 4/Beta
		public string Password { get; set; }
		[Required]
		public string FullName { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "ایمیل شما معتبر نیست")]
		public string Email { get; set; }
		[Required]
		[MinLength(6, ErrorMessage = "حداقل رمز عبور 6 کاراکتر")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "رمز عبور دوم یکسان نیست")]
		public string ConfrimedPassword { get; set; }
		[Required]
		//[StringLength(11, ErrorMessage = "لطفا شماره موبایل خود را صحیح وارد کنید"), MinLength(11, ErrorMessage = "حداقل بایستی 11 کارکترباشد")]
		[DataType(DataType.PhoneNumber)]
		public string Mobile { get; set; }
	}
}