using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CategoryService : IService<CategoryRequest, CategoryResponse, CategoryResponse, int>
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<List<CategoryResponse>> GetList()
		{
			var categoryList = await _categoryRepository.GetList();

			return categoryList.Select(c => new CategoryResponse(c.ID, c.Name, c.Description)).ToList();
		}

		public async Task<CategoryResponse> Get(int id)
		{
			var category = await _categoryRepository.Get(id);

			return category == null ? null! : new CategoryResponse(id, category.Name, category.Description);
		}

		public async Task<CategoryResponse> Create(CategoryRequest category)
		{
			var newCategory = new Category()
			{
				Name = category.Name,
				Description = category.Description,
			};

			var createdCategoryID = await _categoryRepository.Create(newCategory);

			return new CategoryResponse(createdCategoryID, category.Name, category.Description);
		}

		public async Task<CategoryResponse> Update(int id, CategoryRequest updatedCategory)
		{
			var category = new Category()
			{
				Name = updatedCategory.Name,
				Description = updatedCategory.Description,
			};

			await _categoryRepository.Update(id, category);

			return new CategoryResponse(id, updatedCategory.Name, updatedCategory.Description);
		}

		public async Task Delete(int id)
		{
			await _categoryRepository.Delete(id);
		}
	}
}
