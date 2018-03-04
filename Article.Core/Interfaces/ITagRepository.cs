using Article.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Article.Core.Interfaces
{
	public interface ITagRepository
	{
		Task<IEnumerable<Tag>> GetAllAsync();
		Task<Tag> GetAsync(string tagName);
		Task EditAsync(string existingTag, string newTag);
		Task DeleteAsync(string tagName);
		Task CreateAsync(Tag tag);

	}
}
