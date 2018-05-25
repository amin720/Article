﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Article.Areas.User.Models;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Article.Infrastructure.Repository;

namespace Article.Areas.User.Controllers
{
    public class ReportUserController : Controller
    {
		private readonly ICategoryRepository _categoryRepository;
	    private readonly IProductRepository _productRepository;
	    private readonly ICommentRepository _commentRepository;
	    private readonly IUserRepository _userRepository;

	    public ReportUserController()
		    : this(new CategoryRepository(), new ProductRepository(),
			    new CommentRepository(), new UserRepository())

	    { }
	    public ReportUserController(ICategoryRepository categoryRepository, IProductRepository productRepository,
		    ICommentRepository commentRepository, IUserRepository userRepository)
	    {
		    _categoryRepository = categoryRepository;
		    _productRepository = productRepository;
		    _commentRepository = commentRepository;
		    _userRepository = userRepository;
	    }

	    public async Task<ActionResult> Index()
	    {
		    var user = await GetloggedInUser();
		    IEnumerable<Order> orders = new List<Order>();

		    using (var db = new ArticleEntities())
		    {
			    orders = await db.Orders.Where(o => o.Email == user.Email && o.FullName == user.DisplayName).ToListAsync();
		    }

		    return View(orders);
	    }

	    public async Task<ActionResult> Detail(int? id)
	    {
			IList<OrderDetailsViewModel> model = new List<OrderDetailsViewModel>();

		    using (var db = new ArticleEntities())
		    {
			    var orderDetails = await db.OrderDetails.Where(od => od.OrderId == id).ToListAsync();

			    foreach (var item in orderDetails)
			    {
					string productName = String.Empty;

				    if (string.IsNullOrWhiteSpace(item.Product.NamePersian))
				    {
					    productName = item.Product.NameEnglish;
				    }
				    else
				    {
					    productName = item.Product.NamePersian;
				    }
				    model.Add(new OrderDetailsViewModel
				    {
					    NumberOrder = item.OrderId,
						ProductName = productName,
						Qty = item.Quantity,
						UnitPrice = item.Product.Price
				    });
			    }
		    }

		    return View(model);
	    }

	    #region Method

	    private bool _isDisposed;
	    protected override void Dispose(bool disposing)
	    {
		    if (!_isDisposed)
		    {
			    _userRepository.Dispose();
		    }

		    _isDisposed = true;
		    base.Dispose(disposing);
	    }

	    private async Task<UserIdentity> GetloggedInUser()
	    {
		    return await _userRepository.GetUserByNameAsync(User.Identity.Name);
	    }

	    private IList<Category> _categories = new List<Category>();
	    private void CategoryList(Category item)
	    {
		    if (item.Id != null)
		    {
			    _categories.Add(item);

		    }
	    }

	    #endregion
	}
}