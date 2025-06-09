namespace ComputerPartsShop.Domain.Models
{
	public class ShopUser
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string PasswordHash { get; set; }
		public UserRole Role { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiresAtUtc { get; set; }
		public List<UserPaymentSystem> PaymentInfoList { get; set; }
		public List<UserAddress> UserAddresses { get; set; }
		public List<Review>? Reviews { get; set; }
	}
}
