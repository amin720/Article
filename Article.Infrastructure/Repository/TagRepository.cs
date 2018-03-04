using Article.Core.Entities;
using Article.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Infrastructure.Repository
{
	public class TagRepository : ITagRepository
	{
		public async Task<IEnumerable<Tag>> GetAllAsync()
		{
			using (var db = new ArticleEntities())
			{
				return await db.Tags.OrderBy(tag => tag.Name).ToArrayAsync();
			}
		}

		public async Task<Tag> GetAsync(string tagName)
		{
			using (var db = new ArticleEntities())
			{
				var tags = await db.Tags.SingleOrDefaultAsync(tag => tag.Name == tagName);

				if (tags == null)
				{
					throw new KeyNotFoundException("The tag " + tagName + " does not exist.");
				}

				return tags;
			}
		}

		public async Task EditAsync(string existingTag, string newTag)
		{
			using (var db = new ArticleEntities())
			{
				var tags = await db.Tags.SingleOrDefaultAsync(tag => tag.Name == existingTag);

				if (tags == null)
				{
					throw new KeyNotFoundException("The tag " + existingTag + " does not exist.");
				}

				tags.Name = newTag;

				db.SaveChanges();
			}
		}

		public async Task DeleteAsync(string tagName)
		{
			using (var db = new ArticleEntities())
			{
				var tag = await db.Tags.SingleOrDefaultAsync(t => t.Name == tagName);

				if (tag == null)
				{
					throw new KeyNotFoundException("The tag " + tagName + " does not exist.");
				}

				db.Tags.Remove(tag);

				db.SaveChanges();
			}
		}

		public async Task CreateAsync(Tag tag)
		{
			using (var db = new ArticleEntities())
			{
				var tags = await db.Tags.SingleOrDefaultAsync(t => t.Name == tag.Name);

				if (tags != null)
				{
					throw new ArgumentException(message: "A " + tag.Name + " already exsits.");
				}

				db.Tags.Add(tag);

				db.SaveChanges();
			}
		}
	}
}
