namespace ComputerPartsShop.Domain.Models
{
	public class Country
	{
		public int Id { get; set; }
		public string Alpha2 { get; set; }
		public string Alpha3 { get; set; }
		public string Name { get; set; }
		public List<Address> Addresses { get; set; }
	}
}