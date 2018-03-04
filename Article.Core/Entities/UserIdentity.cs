using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Article.Core.Entities
{
	public class UserIdentity : IdentityUser
	{
		[StringLength(100)]
		public string DisplayName { get; set; }
		//[StringLength(100)]
		//public string UserName { get; set; }
		//[DataType(DataType.EmailAddress)]
		//public string Email { get; set; }
	}
}
