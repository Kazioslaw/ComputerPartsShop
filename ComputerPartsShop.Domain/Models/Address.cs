namespace ComputerPartsShop.Domain.Models
{
	public class Address
	{
		public Guid Id { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string ZipCode { get; set; }
		public int CountryId { get; set; }
		public Country Country { get; set; }
		public List<UserAddress> Users { get; set; }
		public List<Order> Orders { get; set; }
	}
}
