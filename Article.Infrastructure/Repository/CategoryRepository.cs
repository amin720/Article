using Article.Core.Entities;
using Article.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Infrastructure.Repository
{
	public class CategoryRepository: ICategoryRepository
	{
		public async Task<Category> GetAsync(int id)
		{
			using (var db = new ArticleEntities())
			{
				var category = await db.Categories.Where(cat => cat.CategoryType == "product")
												.SingleAsync(cat => cat.Id == id);
				if (category == null)
				{
					throw new KeyNotFoundException("The category Id \"" + id + "\" does not exist.");
				}
				return category;
			}
		}

		public async Task<Category> GetAsync(string name)
		{
			using (var db = new ArticleEntities())
			{
				var category = await db.Categories.Where(cat => cat.CategoryType == "product")
					.SingleAsync(cat => cat.Name == name);
				if (category == null)
				{
					throw new KeyNotFoundException("The category Id \"" + name + "\" does not exist.");
				}
				return category;
			}
		}

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			using (var db = new ArticleEntities())
			{
				return await db.Categories
										.Where(cat => cat.CategoryType == "product")
										.Include(cat => cat.Category1)
										.ToListAsync();
			}
		}

		public async Task CreateAsync(Category model)
		{
			using (var db = new ArticleEntities())
			{
				var category = await db.Categories.SingleOrDefaultAsync(cat => cat.Id == model.Id && cat.ParentId == model.ParentId);

				if (category != null)
				{
					throw new ArgumentException("A category with the id of " + model.Id + " already exsits.");
				}

				model.CategoryType = "product";

				db.Categories.Add(model);
				db.SaveChanges();
			}
		}

		public async Task EditAsync(int id, Category updateItem)
		{
			using (var db = new ArticleEntities())
			{
				var category = await db.Categories.SingleOrDefaultAsync(cat => cat.Id == id);

				if (category == null)
				{
					throw new KeyNotFoundException("A category withthe id of " + id + "does not exisst in the data store");
				}

				category.Id = updateItem.Id;
				category.Name = updateItem.Name;
				category.ParentId = updateItem.ParentId;

				db.SaveChanges();
			}
		}

		public async Task DeleteAsync(int id)
		{
			using (var db = new ArticleEntities())
			{
				var category = await db.Categories.SingleOrDefaultAsync(cat => cat.Id == id);

				if (category == null)
				{
					throw new KeyNotFoundException("The catefory with the id of " + id + "does not exist.");
				}

				db.Categories.Remove(category);
				db.SaveChanges();
			}
		}
	}
}
