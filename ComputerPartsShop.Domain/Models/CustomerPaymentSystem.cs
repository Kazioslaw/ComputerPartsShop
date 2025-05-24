namespace ComputerPartsShop.Domain.Models
{
	public class CustomerPaymentSystem
	{
		public Guid ID { get; set; }
		public Guid CustomerID { get; set; }
		public Customer Customer { get; set; }
		public int ProviderID { get; set; }
		public PaymentProvder Provider { get; set; }
		public string PaymentReference { get; set; }
		public List<Payment> Payments { get; set; }
	}
}
