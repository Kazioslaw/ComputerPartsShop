namespace ComputerPartsShop.Models
{
	public class Payment
	{
		public int ID { get; set; }
		public Customer Customer { get; set; }
		public Order Order { get; set; }
		public string PaymentStatus { get; set; }
		public string PaymentType { get; set; }
	}
}
