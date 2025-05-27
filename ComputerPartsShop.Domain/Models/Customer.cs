namespace ComputerPartsShop.Domain.Models
{
	public class Customer
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
		public List<CustomerPaymentSystem> PaymentInfoList { get; set; }
		public List<CustomerAddress> CustomersAddresses { get; set; }
		public List<Review>? Reviews { get; set; }
	}
}
