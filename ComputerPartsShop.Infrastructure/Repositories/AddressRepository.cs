using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class AddressRepository : ICRUDRepository<Address, Guid>
	{
		readonly TempData _dbContext;

		public AddressRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Address>> GetList()
		{
			return _dbContext.AddressList;
		}

		public async Task<Address> Get(Guid id)
		{
			var address = _dbContext.AddressList.FirstOrDefault(x => x.ID == id);

			return address;
		}

		public async Task<Guid> Create(Address request)
		{
			request.ID = Guid.NewGuid();

			_dbContext.AddressList.Add(request);

			return request.ID;
		}

		public async Task<Address> Update(Guid id, Address request)
		{
			var address = _dbContext.AddressList.FirstOrDefault(request => request.ID == id);

			if (address != null)
			{
				address.Street = request.Street;
				address.City = request.City;
				address.Region = request.Region;
				address.ZipCode = request.ZipCode;
				address.CountryID = request.CountryID;
			}

			return address;
		}

		public async Task Delete(Guid id)
		{
			var address = _dbContext.AddressList.FirstOrDefault(a => a.ID == id);

			if (address != null)
			{
				_dbContext.AddressList.Remove(address);
			}
		}
	}
}
