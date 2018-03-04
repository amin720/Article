using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Article.Core.Interfaces;
using Article.Infrastructure.Database;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Article.Infrastructure.Repository
{
	public class RoleRepository : IRoleRepository
	{
		private readonly RoleStore<IdentityRole> _store;
		private readonly RoleManager<IdentityRole> _manager;
		private readonly ContextDb _context = new ContextDb();

		public RoleRepository()
		{
			_store = new RoleStore<IdentityRole>(context: _context);
			_manager = new RoleManager<IdentityRole>(_store);
		}

		public async Task<IdentityRole> GetRoleByNameAsync(string name)
		{
			return await _store.FindByNameAsync(name);
		}

		public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
		{
			return await _store.Roles.ToArrayAsync();
		}

		public async Task CreateAsync(IdentityRole role)
		{
			await _manager.CreateAsync(role);
		}

		private bool _disposed = false;
		public void Dispose()
		{
			if (!_disposed)
			{
				_store.Dispose();
				_manager.Dispose();
			}

			_disposed = true;
		}
	}
}
