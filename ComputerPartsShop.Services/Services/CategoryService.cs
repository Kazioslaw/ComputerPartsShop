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

		public async Task<List<CategoryResponse>> GetListAsync()
		{
			var categoryList = await _categoryRepository.GetListAsync();

			return categoryList.Select(c => new CategoryResponse(c.ID, c.Name, c.Description)).ToList();
		}

		public async Task<CategoryResponse> GetAsync(int id)
		{
			var category = await _categoryRepository.GetAsync(id);

			return category == null ? null! : new CategoryResponse(id, category.Name, category.Description);
		}

		public async Task<CategoryResponse> CreateAsync(CategoryRequest category)
		{
			var newCategory = new Category()
			{
				Name = category.Name,
				Description = category.Description,
			};

			var createdCategoryID = await _categoryRepository.CreateAsync(newCategory);

			return new CategoryResponse(createdCategoryID, category.Name, category.Description);
		}

		public async Task<CategoryResponse> UpdateAsync(int id, CategoryRequest updatedCategory)
		{
			var category = new Category()
			{
				Name = updatedCategory.Name,
				Description = updatedCategory.Description,
			};

			await _categoryRepository.UpdateAsync(id, category);

			return new CategoryResponse(id, updatedCategory.Name, updatedCategory.Description);
		}

		public async Task DeleteAsync(int id)
		{
			await _categoryRepository.DeleteAsync(id);
		}
	}
}
