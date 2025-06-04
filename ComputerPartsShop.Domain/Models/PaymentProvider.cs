namespace ComputerPartsShop.Domain.Models
{
	public class PaymentProvider
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<UserPaymentSystem> UserPayments { get; set; }
	}
}