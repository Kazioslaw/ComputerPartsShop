using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface ICategoryService : IService<CategoryRequest, CategoryResponse, CategoryResponse, int>
	{
	}
}
