using Article.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Article.Core.Interfaces
{
	public interface ICategoryRepository
	{
		Task<Category> GetAsync(int id);
		Task<Category> GetAsync(string name);
		Task<IEnumerable<Category>> GetAllAsync();
		Task CreateAsync(Category model);
		Task EditAsync(int id, Category updateItem);
		Task DeleteAsync(int id);
	}
}
