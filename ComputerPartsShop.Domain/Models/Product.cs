namespace ComputerPartsShop.Domain.Models
{
	public class Product
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal UnitPrice { get; set; }
		public int Stock { get; set; }
		public int CategoryID { get; set; }
		public Category Category { get; set; }
		public string InternalCode { get; set; }
		public List<OrderProduct> Orders { get; set; }
	}
}