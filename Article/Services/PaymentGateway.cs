using Article.ViewModels;
using Braintree;

namespace Article.Services
{
	public class PaymentGateway : IGateway
	{
		/*These API keys have been disabled. Always keep API keys private! Never share them with others or commit them to a public GitHub repository.*/
		private readonly BraintreeGateway _gateway = new BraintreeGateway
		{
			Environment = Braintree.Environment.SANDBOX,
			MerchantId = "vxkg9fw8ynthdkpf",
			PublicKey = "yr5sckdym4t2pfgw",
			PrivateKey = "2ab579bfadfc392ce3b111eef4f6a439"
		};

		public PaymentResult ProcessPayment(CheckoutViewModel model)
		{
			var request = new TransactionRequest()
			{
				Amount = model.Total,
				CreditCard = new TransactionCreditCardRequest()
				{
					Number = model.CardNumber,
					CVV = model.Cvv,
					ExpirationMonth = model.Month,
					ExpirationYear = model.Year
				},
				Options = new TransactionOptionsRequest()
				{
					SubmitForSettlement = true
				}
			};

			var result = _gateway.Transaction.Sale(request);

			if (result.IsSuccess())
			{
				return new PaymentResult(result.Target.Id, true, null);
			}

			return new PaymentResult(null, false, result.Message);
		}
	}
}