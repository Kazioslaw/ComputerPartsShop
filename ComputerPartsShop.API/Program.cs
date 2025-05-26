using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Infrastructure.Interfaces;
using ComputerPartsShop.Services;

namespace ComputerPartsShop
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddOpenApi();
			builder.Services.AddScoped<IService<AddressRequest, AddressResponse, AddressResponse, Guid>, AddressService>();
			builder.Services.AddScoped<IService<CategoryRequest, CategoryResponse, CategoryResponse, int>, CategoryService>();
			builder.Services.AddScoped<IService<CountryRequest, CountryResponse, DetailedCountryResponse, int>, CountryService>();
			builder.Services.AddScoped<IService<CustomerPaymentSystemRequest,
				CustomerPaymentSystemResponse, DetailedCustomerPaymentSystemResponse, Guid>, CustomerPaymentSystemService>();
			builder.Services.AddScoped<IService<CustomerRequest, CustomerResponse,
				DetailedCustomerResponse, Guid>, CustomerService>();
			builder.Services.AddScoped<IService<OrderRequest, OrderResponse, DetailedOrderResponse, int>, OrderService>();
			builder.Services.AddScoped<IService<PaymentRequest, PaymentResponse, DetailedPaymentResponse, int>, PaymentService>();
			builder.Services.AddScoped<IService<PaymentProviderRequest, PaymentProviderResponse, DetailedPaymentProviderResponse, int>, PaymentProviderService>();
			builder.Services.AddScoped<IService<ProductRequest, ProductResponse, ProductResponse, int>, ProductService>();
			builder.Services.AddScoped<IService<ReviewRequest, ReviewResponse, ReviewResponse, int>, ReviewService>();
			builder.Services.AddScoped<IRepository<Address, Guid>, AddressRepository>();
			builder.Services.AddScoped<IRepository<Category, int>, CategoryRepository>();
			builder.Services.AddScoped<IRepository<Country, int>, CountryRepository>();
			builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
			builder.Services.AddScoped<IRepository<CustomerPaymentSystem, Guid>, CustomerPaymentSystemRepository>();
			builder.Services.AddScoped<IRepository<Order, int>, OrderRepository>();
			builder.Services.AddScoped<IRepository<Payment, int>, PaymentRepository>();
			builder.Services.AddScoped<IPaymentProviderRepository, PaymentProviderRepository>();
			builder.Services.AddScoped<IRepository<Product, int>, ProductRepository>();
			builder.Services.AddScoped<IRepository<Review, int>, ReviewRepository>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
