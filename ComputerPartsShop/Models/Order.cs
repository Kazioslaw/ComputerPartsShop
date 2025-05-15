namespace ComputerPartsShop.Models
{
	public class Order
	{
		public int ID { get; set; }
		public Customer Customer { get; set; }
		public List<Product> Product { get; set; }
		public decimal TotalOrder { get; set; }
		public string DeliveryType { get; set; }
		public string DeliveryStatus { get; set; }
	}
}
