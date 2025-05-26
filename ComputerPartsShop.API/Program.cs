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
			builder.Services.AddScoped<IAddressService, AddressService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ICountryService, CountryService>();
			builder.Services.AddScoped<ICustomerPaymentSystemService, CustomerPaymentSystemService>();
			builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IPaymentService, PaymentService>();
			builder.Services.AddScoped<IPaymentProviderService, PaymentProviderService>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IReviewService, ReviewService>();
			builder.Services.AddScoped<IAddressRepository, AddressRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ICountryRepository, CountryRepository>();
			builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
			builder.Services.AddScoped<ICustomerPaymentSystemRepository, CustomerPaymentSystemRepository>();
			builder.Services.AddScoped<IOrderRepository, OrderRepository>();
			builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
			builder.Services.AddScoped<IPaymentProviderRepository, PaymentProviderRepository>();
			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
			builder.Services.AddScoped<TempData>();

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
