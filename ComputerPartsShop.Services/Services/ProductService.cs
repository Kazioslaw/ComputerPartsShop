using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	public class ProductService : ICRUDService<ProductRequest, ProductResponse, ProductResponse, int>
	{
		public List<ProductResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public ProductResponse Get(int id)
		{
			throw new NotImplementedException();
		}

		public ProductResponse Create(ProductRequest request)
		{
			throw new NotImplementedException();
		}

		public ProductResponse Update(int id, ProductRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			return;
		}
	}
}
