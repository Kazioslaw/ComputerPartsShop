using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class AddressRepository : IAddressRepository
	{
		private readonly TempData _dbContext;

		public AddressRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Address>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);

			return _dbContext.AddressList;
		}

		public async Task<Address> GetAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var address = _dbContext.AddressList.FirstOrDefault(x => x.Id == id);

			return address!;
		}

		public async Task<Guid> CreateAsync(Address request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			request.Id = Guid.NewGuid();

			_dbContext.AddressList.Add(request);

			return request.Id;
		}

		public async Task<Address> UpdateAsync(Guid id, Address request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var address = _dbContext.AddressList.FirstOrDefault(request => request.Id == id);

			if (address != null)
			{
				address.Street = request.Street;
				address.City = request.City;
				address.Region = request.Region;
				address.ZipCode = request.ZipCode;
				address.CountryId = request.CountryId;
			}

			return address!;
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var address = _dbContext.AddressList.FirstOrDefault(a => a.Id == id);

			if (address != null)
			{
				_dbContext.AddressList.Remove(address);
			}
		}
	}
}
