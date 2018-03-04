using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Article.Core.Interfaces
{
	public interface IRoleRepository : IDisposable
	{
		Task<IdentityRole> GetRoleByNameAsync(string name);
		Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
		Task CreateAsync(IdentityRole role);
	}
}
