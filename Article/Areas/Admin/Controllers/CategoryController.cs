using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Article.Areas.Admin.ViewModels;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;

namespace Article.Areas.Admin.Controllers
{
	// /admin/Category

	[RouteArea("Admin")]
	[RoutePrefix("Category")]
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _repository;

		public CategoryController()
			: this(new CategoryRepository())
		{

		}

		public CategoryController(ICategoryRepository catRepository)
		{
			_repository = catRepository;
		}

		// GET: Admin/Category
		[Route("")]
		[HttpGet]
		public async Task<ActionResult> Index()
		{
			var categoryies = await _repository.GetAllAsync();

			if (Request.AcceptTypes.Contains("application/json"))
			{
				return Json(categoryies, JsonRequestBehavior.AllowGet);
			}

			if (User.IsInRole("author"))
			{
				return new HttpUnauthorizedResult();
			}

			return View(model: categoryies, viewName: "Index");
		}

		// product: Admin/Category/Create
		[HttpPost]
		[Route("create")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Create(Category category)
		{
			if (!ModelState.IsValid)
			{
				return HttpNotFound();
			}

			//var categories = await _repository.GetAllAsync();

			//Category model = categories.SingleOrDefault(cat => cat.Name == category.Name && cat.ParentId == category.ParentId);

			//if (model != null)
			//{
			//	throw new ArgumentException("A category with the id of " + model.Id + " already exsits.");
			//}

			try
			{
				//model = new Category()
				//{
				//	Name = category.Name,
				//	TypeCategory = "product"
				//};

				//if (category.ParentId != null)
				//{
				//	model.ParentId = category.ParentId;

					
				//}

				await _repository.CreateAsync(category);

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e);
				return View(await _repository.GetAllAsync());
			}

		}

		// Get: Admin/Category/Edit/category
		[HttpGet]
		[Route("edit/{categoryId}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Edit(int categoryId)
		{
			try
			{
				Category category = await _repository.GetAsync(categoryId);

				CategoryViewModel model = new CategoryViewModel
				{
					Id = category.Id,
					Name = category.Name,
					ParentId = (int)category.ParentId,
					Categories = await _repository.GetAllAsync()
				};

				return View(model: model, viewName: "Edit");

			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
		}


		// product: Admin/Category/Edit/tag
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("edit/{cat}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Edit(int cat, CategoryViewModel newCat)
		{
			try
			{
				newCat.Id = cat;
				newCat.Categories = await _repository.GetAllAsync();

				if (string.IsNullOrWhiteSpace(newCat.Name))
				{
					ModelState.AddModelError("key", "New category value cannot be empty.");

					return View(model: newCat);
				}

				Category model = new Category()
				{
					Id = newCat.Id,
					Name = newCat.Name,
					ParentId = newCat.ParentId
				};

				await _repository.EditAsync(cat, model);

				return RedirectToAction("Index");

			}
			catch (AccessViolationException e)
			{
				ModelState.AddModelError("key", e);
				return View(newCat);
			}

		}
		// Get: Admin/Category/Delete
		[HttpGet]
		//        [ValidateAntiForgeryToken]
		[Route("delete/{categoryId}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Delete(int categoryId)
		{
			var model = await _repository.GetAsync(categoryId);
			return View(model: model);
		}

		// product: Admin/Category/Delete
		[HttpPost]
		//        [ValidateAntiForgeryToken]
		[Route("delete/{cat}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Delete(int cat, Category category)
		{
			try
			{
				category.Id = cat;

				await _repository.DeleteAsync(category.Id);

				return RedirectToAction("Index");
			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
		}
		//public PartialViewResult Delete(PostCategory category)
		//{
		//    return (PartialView(model:category,viewName: "_Partial_Change"));
		//}
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public JsonResult Change(int categoryId)
		//{
		//    try
		//    {
		//        _repository.Delete(categoryId);

		//        return Json(JsonRequestBehavior.AllowGet);
		//    }
		//    catch (KeyNotFoundException e)
		//    {
		//        return Json(HttpNotFound());
		//    }
		//}
	}
}
