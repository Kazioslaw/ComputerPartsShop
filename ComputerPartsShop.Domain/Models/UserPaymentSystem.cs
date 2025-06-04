namespace ComputerPartsShop.Domain.Models
{
	public class UserPaymentSystem
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public ShopUser User { get; set; }
		public int ProviderId { get; set; }
		public PaymentProvider Provider { get; set; }
		public string PaymentReference { get; set; }
		public List<Payment> Payments { get; set; }
	}
}
