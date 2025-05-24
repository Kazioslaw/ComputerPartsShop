namespace ComputerPartsShop.Domain.Models
{
	public class Order
	{
		public int ID { get; set; }
		public Guid CustomerID { get; set; }
		public Customer Customer { get; set; }
		public List<OrderProduct> OrdersProducts { get; set; }
		public Guid DeliveryAddressID { get; set; }
		public Address DeliveryAddress { get; set; }
		public string Status { get; set; }
		public DateTime OrderedAt { get; set; }
		public List<Payment> Payments { get; set; }
		public DateTime? SendAt { get; set; }
	}
}
