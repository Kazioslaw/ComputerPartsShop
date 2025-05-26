using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IProductService : IService<ProductRequest, ProductResponse, ProductResponse, int>
	{
	}
}
