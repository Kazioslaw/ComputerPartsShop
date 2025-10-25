namespace ComputerPartsShop.Domain.Models
{
	public class UserAddress
	{
		public Guid UserId { get; set; }
		public ShopUser User { get; set; }
		public Guid AddressId { get; set; }
		public Address Address { get; set; }
	}
}