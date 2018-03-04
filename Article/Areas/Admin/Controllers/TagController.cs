using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Article.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("Tag")]
	[Authorize]
	public class TagController : Controller
	{
		private readonly ITagRepository _repository;

		public TagController()
			: this(new TagRepository())
		{
		}

		public TagController(ITagRepository repository)
		{
			_repository = repository;
		}

		// GET: Admin/Tag
		[HttpGet]
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var tags = await _repository.GetAllAsync();

			if (Request.AcceptTypes.Contains("application/json"))
			{
				return Json(tags, JsonRequestBehavior.AllowGet);
			}

			if (User.IsInRole("author"))
			{
				return new HttpUnauthorizedResult();
			}

			return View(tags);
		}


		// Get: Admin/Tag/Edit/tag
		[HttpGet]
		[Route("edit/{tag}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Edit(string tag)
		{
			try
			{
				var model = await _repository.GetAsync(tag);
				return View(model: model.Name);
			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
		}
		//public JsonResult Edit(string tag)
		//{

		//    var model = _repository.Get(tag);
		//    return Json(model);
		//}


		// product: Admin/Tag/Edit/tag
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("edit/{tag}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Edit(string tag, string newTag)
		{
			var tags = await _repository.GetAllAsync();

			var exist = tags as IList<Tag> ?? tags.ToList();
			if (!exist.Contains(new Tag{Name = tag}))
			{
				return HttpNotFound();
			}

			if (exist.Contains(new Tag { Name = newTag }))
			{
				return RedirectToAction("Index");
			}

			if (string.IsNullOrWhiteSpace(newTag))
			{
				ModelState.AddModelError("key", "New tag value cannot be empty.");

				return View(model: tag);
			}

			await _repository.EditAsync(tag, newTag);

			return RedirectToAction("Index");
		}


		// Get: Admin/Tag/Delete/tag
		[HttpGet]
		[Route("delete/{tag}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Delete(string tag)
		{
			try
			{
				var model = await _repository.GetAsync(tag);
				return View(model: model.Name);
			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
		}

		// product: Admin/Tag/Delete/tag
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("delete/{tag}")]
		[Authorize(Roles = "admin, editor")]
		public async Task<ActionResult> Delete(string tag, string foo)
		{
			try
			{
				await _repository.DeleteAsync(tag);

				return RedirectToAction("Index");
			}
			catch (KeyNotFoundException e)
			{
				return HttpNotFound();
			}
		}

		// product: Admin/Tag/tag
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin, editor")]
		public async Task<JsonResult> Create(Tag tag)
		{
			var tags = await _repository.GetAllAsync();

			var exist = tags as IList<Tag> ?? tags.ToList();
			if (exist.Contains(tag))
			{
				ModelState.AddModelError("key", "Tag is exiting.");

				return Json(RedirectToAction("Index"));
			}

			if (string.IsNullOrWhiteSpace(tag.Name))
			{
				ModelState.AddModelError("key", "New tag value cannot be empty.");

				return Json(RedirectToAction("Index"));
			}

			await _repository.CreateAsync(tag);

			return Json(tag);
		}
	}
}