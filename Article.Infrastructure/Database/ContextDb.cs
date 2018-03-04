using System.Data.Entity;
using Article.Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Article.Infrastructure.Database
{
	public class ContextDb : IdentityDbContext<UserIdentity>
	{
		public ContextDb()
			: base("name=Article")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
