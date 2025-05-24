using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	internal class CategoryService : ICRUDService<CategoryRequest, CategoryResponse, CategoryResponse, int>
	{
		public List<CategoryResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public CategoryResponse Get(int id)
		{
			throw new NotImplementedException();
		}
		public CategoryResponse Create(CategoryRequest request)
		{
			throw new NotImplementedException();
		}

		public CategoryResponse Update(int id, CategoryRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
