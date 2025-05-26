namespace ComputerPartsShop.Domain.Models
{
	public class Review
	{
		public int Id { get; set; }
		public Guid? CustomerId { get; set; }
		public Customer? Customer { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public byte Rating { get; set; }
		public string? Description { get; set; }
	}
}
