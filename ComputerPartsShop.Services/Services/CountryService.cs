using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	public class CountryService : ICRUDService<CountryRequest, CountryResponse, DetailedCountryResponse, int>
	{
		public List<CountryResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public DetailedCountryResponse Get(int id)
		{
			throw new NotImplementedException();
		}

		public CountryResponse Create(CountryRequest request)
		{
			throw new NotImplementedException();
		}

		public CountryResponse Update(int id, CountryRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			return;
		}

	}
}
