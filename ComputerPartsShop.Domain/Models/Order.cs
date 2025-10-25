using ComputerPartsShop.Domain.Enums;

namespace ComputerPartsShop.Domain.Models
{
	public class Order
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public ShopUser User { get; set; }
		public List<OrderProduct> OrdersProducts { get; set; }
		public decimal Total { get; set; }
		public Guid DeliveryAddressId { get; set; }
		public Address DeliveryAddress { get; set; }
		public DeliveryStatus Status { get; set; }
		public DateTime OrderedAt { get; set; }
		public DateTime? SendAt { get; set; }
		public List<Payment> Payments { get; set; }
	}
}
