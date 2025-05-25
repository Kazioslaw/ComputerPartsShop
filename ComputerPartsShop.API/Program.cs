using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
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
			builder.Services.AddScoped<ICRUDService<AddressRequest, AddressResponse, AddressResponse, Guid>, AddressService>();
			builder.Services.AddScoped<ICRUDService<CategoryRequest, CategoryResponse, CategoryResponse, int>, CategoryService>();
			builder.Services.AddScoped<ICRUDService<CountryRequest, CountryResponse, DetailedCountryResponse, int>, CountryService>();
			builder.Services.AddScoped<ICRUDService<CustomerPaymentSystemRequest,
				CustomerPaymentSystemResponse, DetailedCustomerPaymentSystemResponse, Guid>, CustomerPaymentSystemService>();
			builder.Services.AddScoped<ICRUDService<CustomerRequest, CustomerResponse,
				DetailedCustomerResponse, Guid>, CustomerService>();
			builder.Services.AddScoped<ICRUDService<OrderRequest, OrderResponse, DetailedOrderResponse, int>, OrderService>();
			builder.Services.AddScoped<ICRUDService<PaymentRequest, PaymentResponse, DetailedPaymentResponse, int>, PaymentService>();
			builder.Services.AddScoped<ICRUDService<PaymentProviderRequest, PaymentProviderResponse, DetailedPaymentProviderResponse, int>, PaymentProviderService>();
			builder.Services.AddScoped<ICRUDService<ProductRequest, ProductResponse, ProductResponse, int>, ProductService>();
			builder.Services.AddScoped<ICRUDService<ReviewRequest, ReviewResponse, ReviewResponse, int>, ReviewService>();
			builder.Services.AddScoped<ICRUDRepository<Address, Guid>, AddressRepository>();
			builder.Services.AddScoped<ICRUDRepository<Category, int>, CategoryRepository>();
			builder.Services.AddScoped<ICRUDRepository<Country, int>, CountryRepository>();
			builder.Services.AddScoped<ICRUDRepository<Customer, Guid>, CustomerRepository>();
			builder.Services.AddScoped<ICRUDRepository<CustomerPaymentSystem, Guid>, CustomerPaymentSystemRepository>();
			builder.Services.AddScoped<ICRUDRepository<Order, int>, OrderRepository>();
			builder.Services.AddScoped<ICRUDRepository<Payment, int>, PaymentRepository>();
			builder.Services.AddScoped<ICRUDRepository<PaymentProvider, int>, PaymentProviderRepository>();
			builder.Services.AddScoped<ICRUDRepository<Product, int>, ProductRepository>();
			builder.Services.AddScoped<ICRUDRepository<Review, int>, ReviewRepository>();

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
