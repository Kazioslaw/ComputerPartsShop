using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class ProductService : ICRUDService<ProductRequest, ProductResponse, ProductResponse, int>
	{
		private readonly CategoryRepository _categoryRepository;
		private readonly ProductRepository _productRepository;

		public ProductService(ProductRepository productRepository, CategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<List<ProductResponse>> GetList()
		{
			var productList = await _productRepository.GetList();

			return productList.Select(p => new ProductResponse(p.ID, p.Name, p.Description, p.UnitPrice, p.Stock, p.Category.Name, p.InternalCode)).ToList();
		}

		public async Task<ProductResponse> Get(int id)
		{
			var product = await _productRepository.Get(id);

			return product == null ? null! :
				new ProductResponse(product.ID, product.Name, product.Description, product.UnitPrice, product.Stock, product.Category.Name, product.InternalCode);
		}

		public async Task<ProductResponse> Create(ProductRequest product)
		{
			var category = await _categoryRepository.GetByName(product.CategoryName);

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

			var productID = await _productRepository.Create(newProduct);

			return new ProductResponse(productID, product.Name, product.Description, product.UnitPrice, product.Stock, product.CategoryName, product.InternalCode);
		}

		public async Task<ProductResponse> Update(int id, ProductRequest updatedProduct)
		{
			var category = await _categoryRepository.GetByName(updatedProduct.CategoryName);

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

			await _productRepository.Update(id, product);

			return new ProductResponse(id, updatedProduct.Name, updatedProduct.Description, updatedProduct.UnitPrice,
				updatedProduct.Stock, updatedProduct.CategoryName, updatedProduct.InternalCode);
		}

		public async Task Delete(int id)
		{
			await _productRepository.Delete(id);
		}
	}
}
