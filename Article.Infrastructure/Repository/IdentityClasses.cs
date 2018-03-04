using Article.Core.Entities;
using Article.Infrastructure.Database;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Article.Infrastructure.Repository
{
	public class CmsUserStore : UserStore<UserIdentity>
	{
		public CmsUserStore()
			: this(new ContextDb())
		{ }
		public CmsUserStore(ContextDb context)
			: base(context)
		{ }

	}

	public class CmsUserManager : UserManager<UserIdentity>
	{
		public CmsUserManager()
			: this(new CmsUserStore())
		{ }

		public CmsUserManager(UserStore<UserIdentity> userStore)
			: base(userStore)
		{ }
	}
}
