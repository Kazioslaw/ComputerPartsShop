using ComputerPartsShop.Domain.DTOs;


namespace ComputerPartsShop.Services
{
	public class AddressService : ICRUDService<AddressRequest, AddressResponse, AddressResponse, Guid>
	{
		public List<AddressResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public AddressResponse Get(Guid id)
		{
			throw new NotImplementedException();
		}

		public AddressResponse Create(AddressRequest address)
		{
			throw new NotImplementedException();
		}

		public AddressResponse Update(Guid id, AddressRequest address)
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
		}
	}
}
