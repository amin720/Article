using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Article.Core.Entities;
using Article.Core.Interfaces;

namespace Article.Infrastructure.Repository
{
	public class CommentRepository : ICommentRepository
	{
		public async Task<Comment> GetByIdAsync(int commentId)
		{
			using (var db = new ArticleEntities())
			{
				var model = await db.Comments.SingleOrDefaultAsync(c => c.Id == commentId);

				return model;
			}
		}

		public async Task<IEnumerable<Comment>> GetAllAsynce()
		{
			using (var db = new ArticleEntities())
			{
				return await db.Comments.OrderBy(c => c.Id).ToListAsync();
			}
		}

		public async Task<IEnumerable<Comment>> GetAllByProductId(int productId)
		{
			using (var db = new ArticleEntities())
			{
				var model = await db.Comments.Where(c => c.ProductId == productId).ToListAsync();

				return model;
			}
		}

		public async Task CreateAsync(Comment model)
		{
			using (var db = new ArticleEntities())
			{
				//var exist = await db.Comments.SingleOrDefaultAsync(c => c.Title == model.Title);

				//if (exist != null)
				//{
				//	throw new ArgumentException("A Comment with the name of " + model.Title + " already exsits.");
				//}

				db.Comments.Add(model);
				await db.SaveChangesAsync();
			}
		}
		public async Task EditAsync(int id, Comment newModel)
		{
			using (var db = new ArticleEntities())
			{
				var exist = await db.Comments.SingleOrDefaultAsync(c => c.Title == newModel.Title);

				if (exist == null)
				{
					throw new ArgumentException("A Comment with the name of " + newModel.Title + " doesnot already exsits.");
				}

				exist.Title = newModel.Title;
				exist.Email = newModel.Email;
				exist.Name = newModel.Name;
				exist.Desciption = newModel.Desciption;
				exist.Price = newModel.Price;
				exist.Vakue = newModel.Vakue;
				exist.ProductId = newModel.ProductId;

				await db.SaveChangesAsync();
			}
		}
		public async Task DeleteAsync(string title, int productId)
		{
			using (var db = new ArticleEntities())
			{
				var exist = await db.Comments.SingleOrDefaultAsync(c => c.Title == title && c.ProductId == productId);

				if (exist == null)
				{
					throw new ArgumentException("A Comment with the name of " + title + " doesnot already exsits.");
				}

				db.Comments.Remove(exist);
				await db.SaveChangesAsync();
			}
		}
	}
}
