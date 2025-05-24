namespace ComputerPartsShop.Domain.Models
{
	public class CustomerAddress
	{
		public Guid CustomerID { get; set; }
		public Customer Customer { get; set; }
		public Guid AddressID { get; set; }
		public Address Address { get; set; }
	}
}