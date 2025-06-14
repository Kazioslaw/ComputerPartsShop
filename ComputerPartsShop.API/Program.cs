using ComputerPartsShop.API.Validators;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using System.Text.Json.Serialization;

namespace ComputerPartsShop
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.			
			var connectionString = builder.Configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");
			builder.Services.AddScoped<DBContext>(sp => new DBContext(connectionString));
			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

			builder.Services.AddAutoMapper(typeof(Program));

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddOpenApi();
			builder.Services.AddSwaggerGen(c =>
			{
				var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
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

			builder.Services.AddScoped<IValidator<AddressRequest>, AddressRequestValidator>();
			builder.Services.AddScoped<IValidator<UpdateAddressRequest>, UpdateAddressRequestValidator>();
			builder.Services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidator>();
			builder.Services.AddScoped<IValidator<CountryRequest>, CountryRequestValidator>();
			builder.Services.AddScoped<IValidator<CustomerPaymentSystemRequest>, CustomerPaymentSystemRequestValidator>();
			builder.Services.AddScoped<IValidator<CustomerRequest>, CustomerRequestValidator>();
			builder.Services.AddScoped<IValidator<OrderRequest>, OrderRequestValidator>();
			builder.Services.AddScoped<IValidator<UpdateOrderRequest>, UpdateOrderRequestValidator>();
			builder.Services.AddScoped<IValidator<PaymentProviderRequest>, PaymentProviderRequestValidator>();
			builder.Services.AddScoped<IValidator<PaymentRequest>, PaymentRequestValidator>();
			builder.Services.AddScoped<IValidator<UpdatePaymentRequest>, UpdatePaymentRequestValidator>();
			builder.Services.AddScoped<IValidator<ProductRequest>, ProductRequestValidator>();
			builder.Services.AddScoped<IValidator<ReviewRequest>, ReviewRequestValidator>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
