using Article.ViewModels;

namespace Article.Services
{
	public interface IGateway
	{
		PaymentResult ProcessPayment(CheckoutViewModel model);
	}
}
