using Article.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Article.ViewModels
{
	public class CheckoutViewModel
	{
		[Required]
		[Display(Name = "نام کوچک")]
		[StringLength(160)]
		public string FirstName { get; set; }

		//[Required]
		//[Display(Name = "Last Name")]
		//[StringLength(160)]
		//public string LastName { get; set; }

		//[Required]
		//[StringLength(70, MinimumLength = 3)]
		//public string Address { get; set; }

		//[Required]
		//[StringLength(40)]
		//public string City { get; set; }

		//[Required]
		//[StringLength(40)]
		//public string State { get; set; }

		//[Required]
		//[Display(Name = "Postal Code")]
		//[StringLength(10, MinimumLength = 5)]
		//public string PostalCode { get; set; }

		//[Required]
		//[StringLength(40)]
		//public string Country { get; set; }

		[Required]
		[StringLength(24)]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Required]
		[Display(Name = "Email Address")]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public decimal Total { get; set; }

		//[Required]
		public string CardNumber { get; set; }

		//[Required]
		public string Cvv { get; set; }

		//[Required]
		public string Month { get; set; }

		//[Required]
		public string Year { get; set; }

		public string UserId { get; set; }
		public IEnumerable<CartItem> CartItems { get; set; }
		public decimal? Discount { get; set; }
		public decimal Subtotal { get; set; }
	}
}