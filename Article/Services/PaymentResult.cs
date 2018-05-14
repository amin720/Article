namespace Article.Services
{
	public class PaymentResult
	{
		public PaymentResult(string transactionId, bool success, string message)
		{
			TransactionId = transactionId;
			Succeeded = success;
			Message = message;
		}

		public string TransactionId { get; set; }
		public bool Succeeded { get; set; }
		public string Message { get; private set; }
	}
}