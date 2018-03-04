using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Microsoft.AspNet.Identity;

namespace Article.Infrastructure.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly CmsUserStore _store;
		private readonly CmsUserManager _manager;

		public UserRepository()
		{
			_store = new CmsUserStore();
			_manager = new CmsUserManager(_store);
		}

		public async Task<UserIdentity> GetUserByNameAsync(string username)
		{
			return await _store.FindByNameAsync(username);
		}

		public async Task<IEnumerable<UserIdentity>> GetAllUsersAsync()
		{
			return await _store.Users.ToArrayAsync();
		}

		public async Task CreateAsync(UserIdentity user, string password)
		{
			await _manager.CreateAsync(user, password);
		}

		public async Task DeleteAsync(UserIdentity user)
		{
			await _manager.DeleteAsync(user);
		}

		public async Task UpdateAsync(UserIdentity user)
		{
			await _manager.UpdateAsync(user);
		}

		public bool VerifyUserPassword(string hashedPassword, string providedPassword)
		{
			return _manager.PasswordHasher.VerifyHashedPassword(hashedPassword, providedPassword) ==
				   PasswordVerificationResult.Success;
		}

		public string HashPassword(string password)
		{
			return _manager.PasswordHasher.HashPassword(password);
		}

		public async Task AddUserToRoleAsync(UserIdentity user, string role)
		{
			await _manager.AddToRoleAsync(user.Id, role);
		}

		public async Task<IEnumerable<string>> GetRolesForUserAsync(UserIdentity user)
		{
			return await _manager.GetRolesAsync(user.Id);
		}

		public async Task RemoveUserFromRoleAsync(UserIdentity user, params string[] roleNames)
		{
			await _manager.RemoveFromRolesAsync(user.Id, roleNames);
		}

		public async Task<UserIdentity> GetLoginUserAsync(string username, string password)
		{
			return await _manager.FindAsync(username, password);
		}
		public async Task<ClaimsIdentity> CreateIdentityAsync(UserIdentity user)
		{
			return await _manager.CreateIdentityAsync(
				user, DefaultAuthenticationTypes.ApplicationCookie);
		}

		private bool _disposed = false;

		public void Dispose()
		{
			if (!_disposed)
			{
				_manager.Dispose();
				_store.Dispose();
			}

			_disposed = true;
		}
	}
}


