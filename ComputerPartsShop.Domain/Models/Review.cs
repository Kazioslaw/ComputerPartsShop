namespace ComputerPartsShop.Domain.Models
{
	public class Review
	{
		public int ID { get; set; }
		public Guid? CustomerID { get; set; }
		public Customer? Customer { get; set; }
		public int ProductID { get; set; }
		public Product Product { get; set; }
		public byte Rating { get; set; }
		public string? Description { get; set; }
	}
}
