namespace ComputerPartsShop.Domain.Models
{
	public class Payment
	{
		public int Id { get; set; }
		public Guid CustomerPaymentSystemId { get; set; }
		public CustomerPaymentSystem CustomerPaymentSystem { get; set; }
		public int OrderId { get; set; }
		public Order Order { get; set; }
		public decimal Total { get; set; }
		public string Method { get; set; }
		public string Status { get; set; }
		public DateTime PaymentStartAt { get; set; }
		public DateTime? PaidAt { get; set; }
	}
}