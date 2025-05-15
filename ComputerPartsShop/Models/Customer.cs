namespace ComputerPartsShop.Models
{
	public class Customer
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public Address ContactAddress { get; set; }
		public Address DeliveryAddress { get; set; }
	}
}
