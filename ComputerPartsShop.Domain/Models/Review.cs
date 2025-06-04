namespace ComputerPartsShop.Domain.Models
{
	public class Review
	{
		public int Id { get; set; }
		public Guid? UserId { get; set; }
		public ShopUser? User { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public byte Rating { get; set; }
		public string? Description { get; set; }
	}
}
