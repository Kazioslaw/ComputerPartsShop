using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<List<CategoryResponse>> GetListAsync(CancellationToken ct)
		{
			var categoryList = await _categoryRepository.GetListAsync(ct);

			return categoryList.Select(c => new CategoryResponse(c.Id, c.Name, c.Description)).ToList();
		}

		public async Task<CategoryResponse> GetAsync(int id, CancellationToken ct)
		{
			var category = await _categoryRepository.GetAsync(id, ct);

			return category == null ? null! : new CategoryResponse(id, category.Name, category.Description);
		}

		public async Task<CategoryResponse> CreateAsync(CategoryRequest entity, CancellationToken ct)
		{
			var newCategory = new Category()
			{
				Name = entity.Name,
				Description = entity.Description,
			};

			var createdCategoryId = await _categoryRepository.CreateAsync(newCategory, ct);

			return new CategoryResponse(createdCategoryId, entity.Name, entity.Description);
		}

		public async Task<CategoryResponse> UpdateAsync(int id, CategoryRequest entity, CancellationToken ct)
		{
			var category = new Category()
			{
				Name = entity.Name,
				Description = entity.Description,
			};

			await _categoryRepository.UpdateAsync(id, category, ct);

			return new CategoryResponse(id, entity.Name, entity.Description);
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _categoryRepository.DeleteAsync(id, ct);
		}
	}
}
