using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class AddressRepository : ICRUDRepository<Address, Guid>
	{
		public List<Address> GetList()
		{
			throw new NotImplementedException();
		}

		public Address Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public Address Create(Address request)
		{
			throw new NotImplementedException();
		}

		public Address Update(Guid id, Address request)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{

		}
	}
}
