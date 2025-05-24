namespace ComputerPartsShop.Domain.Models
{
	public class Country
	{
		public int ID { get; set; }
		public string Alpha2 { get; set; }
		public string Alpha3 { get; set; }
		public string Name { get; set; }
		public List<Address> Addresses { get; set; }
	}
}