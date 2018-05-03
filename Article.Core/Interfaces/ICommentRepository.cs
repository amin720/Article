using System.Collections.Generic;
using System.Threading.Tasks;
using Article.Core.Entities;

namespace Article.Core.Interfaces
{
	public interface ICommentRepository
	{
		Task<Comment> GetByIdAsync(int commentId);

		Task<IEnumerable<Comment>> GetAllAsynce();

		Task<IEnumerable<Comment>> GetAllByProductId(int productId);

		Task CreateAsync(Comment model);
		Task EditAsync(int id, Comment newModel);
		Task DeleteAsync(string title, int productId);
	}
}
