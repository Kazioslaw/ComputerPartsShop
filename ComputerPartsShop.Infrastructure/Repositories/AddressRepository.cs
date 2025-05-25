using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class AddressRepository : ICRUDRepository<Address, Guid>
	{
		public Task<List<Address>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Address> Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<Guid> Create(Address request)
		{
			throw new NotImplementedException();
		}

		public Task<Address> Update(Guid id, Address request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(Guid id)
		{
			return Task.CompletedTask;
		}
	}
}
