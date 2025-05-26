namespace ComputerPartsShop.Domain.Models
{
	public class CustomerAddress
	{
		public Guid CustomerId { get; set; }
		public Customer Customer { get; set; }
		public Guid AddressId { get; set; }
		public Address Address { get; set; }
	}
}