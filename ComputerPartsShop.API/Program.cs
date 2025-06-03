using ComputerPartsShop.API;
using ComputerPartsShop.Infrastructure;
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

			builder.Services.AddApplicationServices();
			builder.Services.AddApplicationRepositories();

			builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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
