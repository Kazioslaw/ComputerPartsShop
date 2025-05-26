using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class ProductService : IService<ProductRequest, ProductResponse, ProductResponse, int>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IRepository<Product, int> _productRepository;

		public ProductService(IRepository<Product, int> productRepository, ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<List<ProductResponse>> GetListAsync()
		{
			var productList = await _productRepository.GetListAsync();

			return productList.Select(p => new ProductResponse(p.ID, p.Name, p.Description, p.UnitPrice, p.Stock, p.Category.Name, p.InternalCode)).ToList();
		}

		public async Task<ProductResponse> GetAsync(int id)
		{
			var product = await _productRepository.GetAsync(id);

			return product == null ? null! :
				new ProductResponse(product.ID, product.Name, product.Description, product.UnitPrice, product.Stock, product.Category.Name, product.InternalCode);
		}

		public async Task<ProductResponse> CreateAsync(ProductRequest product)
		{
			var category = await _categoryRepository.GetByNameAsync(product.CategoryName);

			var newProduct = new Product()
			{
				Name = product.Name,
				Description = product.Description,
				UnitPrice = product.UnitPrice,
				Stock = product.Stock,
				CategoryID = category.ID,
				Category = category,
				InternalCode = product.InternalCode,
			};

			var productID = await _productRepository.CreateAsync(newProduct);

			return new ProductResponse(productID, product.Name, product.Description, product.UnitPrice, product.Stock, product.CategoryName, product.InternalCode);
		}

		public async Task<ProductResponse> UpdateAsync(int id, ProductRequest updatedProduct)
		{
			var category = await _categoryRepository.GetByNameAsync(updatedProduct.CategoryName);

			var product = new Product()
			{
				Name = updatedProduct.Name,
				Description = updatedProduct.Description,
				UnitPrice = updatedProduct.UnitPrice,
				Stock = updatedProduct.Stock,
				CategoryID = category.ID,
				Category = category,
				InternalCode = updatedProduct.InternalCode,
			};

			await _productRepository.UpdateAsync(id, product);

			return new ProductResponse(id, updatedProduct.Name, updatedProduct.Description, updatedProduct.UnitPrice,
				updatedProduct.Stock, updatedProduct.CategoryName, updatedProduct.InternalCode);
		}

		public async Task DeleteAsync(int id)
		{
			await _productRepository.DeleteAsync(id);
		}
	}
}
