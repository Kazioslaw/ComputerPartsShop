namespace ComputerPartsShop.Domain.Models
{
	public class CustomerPaymentSystem
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public Customer Customer { get; set; }
		public int ProviderId { get; set; }
		public PaymentProvider Provider { get; set; }
		public string PaymentReference { get; set; }
		public List<Payment> Payments { get; set; }
	}
}
