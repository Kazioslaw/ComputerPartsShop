using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class ProductService : IProductService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<List<ProductResponse>> GetListAsync(CancellationToken ct)
		{
			var productList = await _productRepository.GetListAsync(ct);

			return productList.Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.UnitPrice, p.Stock, p.Category.Name, p.InternalCode)).ToList();
		}

		public async Task<ProductResponse> GetAsync(int id, CancellationToken ct)
		{
			var product = await _productRepository.GetAsync(id, ct);

			return product == null ? null! :
				new ProductResponse(product.Id, product.Name, product.Description, product.UnitPrice, product.Stock, product.Category.Name, product.InternalCode);
		}

		public async Task<ProductResponse> CreateAsync(ProductRequest entity, CancellationToken ct)
		{
			var category = await _categoryRepository.GetByNameAsync(entity.CategoryName, ct);

			var newProduct = new Product()
			{
				Name = entity.Name,
				Description = entity.Description,
				UnitPrice = entity.UnitPrice,
				Stock = entity.Stock,
				CategoryId = category.Id,
				Category = category,
				InternalCode = entity.InternalCode,
			};

			var productId = await _productRepository.CreateAsync(newProduct, ct);

			return new ProductResponse(productId, entity.Name, entity.Description, entity.UnitPrice, entity.Stock, entity.CategoryName, entity.InternalCode);
		}

		public async Task<ProductResponse> UpdateAsync(int id, ProductRequest entity, CancellationToken ct)
		{
			var category = await _categoryRepository.GetByNameAsync(entity.CategoryName, ct);

			var product = new Product()
			{
				Name = entity.Name,
				Description = entity.Description,
				UnitPrice = entity.UnitPrice,
				Stock = entity.Stock,
				CategoryId = category.Id,
				Category = category,
				InternalCode = entity.InternalCode,
			};

			await _productRepository.UpdateAsync(id, product, ct);

			return new ProductResponse(id, entity.Name, entity.Description, entity.UnitPrice,
				entity.Stock, entity.CategoryName, entity.InternalCode);
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _productRepository.DeleteAsync(id, ct);
		}
	}
}
