using Article.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Article.Core.Interfaces
{
	public interface IProductRepository
	{
		Task<Product> GetByIdAsync(int id);
		Task<IEnumerable<Product>> GetAllByCategoryAsync(string category);
		Task<IEnumerable<Product>> GetAllByUserAsync(string userId);
		Task<IEnumerable<Product>> GetAllAsync();
		Task CreateAsync(Product model);
		Task EditAsync(int id, Product updateItem);
		Task DeleteAsync(int id);
		Task<IEnumerable<Product>> GetPageAsync(int pageNumber, int pageSize);
		Task<IEnumerable<Product>> GetPageByCategoryTypeAsync(int pageNumber, int pageSize, string categoryType);
		Task<IEnumerable<Product>> GetPageByCategoryAsync(int pageNumber, int pageSize, string category);
		Task<IEnumerable<Product>> GetPageByCategoryAsync(int pageNumber, int pageSize, int categoryId);
		Task<IEnumerable<Product>> GetProductsByAuthorAsync(string authorId);
		Task<IEnumerable<Product>> GetPublishedProductsAsync();
	}
}
