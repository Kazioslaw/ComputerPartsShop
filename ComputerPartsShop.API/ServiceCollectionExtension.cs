using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;

namespace ComputerPartsShop.API
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IAddressService, AddressService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<ICountryService, CountryService>();
			services.AddScoped<ICustomerPaymentSystemService, CustomerPaymentSystemService>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped<IPaymentProviderService, PaymentProviderService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IReviewService, ReviewService>();

			return services;
		}

		public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
		{
			services.AddScoped<IAddressRepository, AddressRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<ICustomerPaymentSystemRepository, CustomerPaymentSystemRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IPaymentRepository, PaymentRepository>();
			services.AddScoped<IPaymentProviderRepository, PaymentProviderRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IReviewRepository, ReviewRepository>();

			return services;
		}
	}
}
