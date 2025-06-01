using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.Models
{
	public class Payment
	{
		public Guid Id { get; set; }
		public Guid CustomerPaymentSystemId { get; set; }
		public CustomerPaymentSystem CustomerPaymentSystem { get; set; }
		public int OrderId { get; set; }
		public Order Order { get; set; }
		public decimal Total { get; set; }
		public PaymentMethod Method { get; set; }
		public PaymentStatus Status { get; set; }
		public DateTime PaymentStartAt { get; set; }
		public DateTime? PaidAt { get; set; }
	}
}