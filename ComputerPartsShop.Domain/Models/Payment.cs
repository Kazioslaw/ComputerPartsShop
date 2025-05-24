namespace ComputerPartsShop.Domain.Models
{
	public class Payment
	{
		public int ID { get; set; }
		public Guid CustomerPaymentSystemID { get; set; }
		public CustomerPaymentSystem CustomerPaymentSystem { get; set; }
		public int OrderID { get; set; }
		public Order Order { get; set; }
		public decimal Total { get; set; }
		public string Method { get; set; }
		public string Status { get; set; }
		public DateTime PaymentStartAt { get; set; }
		public DateTime? PaidAt { get; set; }
	}
}