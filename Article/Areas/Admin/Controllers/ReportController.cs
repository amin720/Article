using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;

namespace Article.Areas.Admin.Controllers
{
	public class ReportController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductRepository _productRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IUserRepository _userRepository;

		public ReportController()
			: this(new CategoryRepository(), new ProductRepository(),
				   new CommentRepository(), new UserRepository())

		{ }
		public ReportController(ICategoryRepository categoryRepository, IProductRepository productRepository,
								ICommentRepository commentRepository, IUserRepository userRepository)
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
			_commentRepository = commentRepository;
			_userRepository = userRepository;
		}

		// GET: Admin/Report
		public ActionResult Index()
		{
			return View();
		}
	}
}