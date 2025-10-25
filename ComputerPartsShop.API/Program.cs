using ComputerPartsShop.API;
using ComputerPartsShop.Domain;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Infrastructure.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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

			builder.Services.Configure<JwtOptions>(
				builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

			builder.Services.AddScoped<DBContext>(sp => new DBContext(connectionString));
			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});
			builder.Services.AddAutoMapper(typeof(Program));

			builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
			builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();

			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.ConfigureAll<BearerTokenOptions>(options =>
			{
				options.BearerTokenExpiration = TimeSpan.FromMinutes(15);
			});

			builder.Services.AddOpenApi();
			builder.Services.AddSwaggerGen(cfg =>
			{
				var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				cfg.IncludeXmlComments(xmlPath);

				cfg.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Enter proper JWT Token",
					Name = "Authorization",
					Scheme = "Bearer",
					BearerFormat = "JWT",
					Type = SecuritySchemeType.Http
				});

				cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "BearerAuth"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				var jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey).Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtOptions.Issuer,
					ValidAudience = jwtOptions.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
				};
			});

			builder.Services.AddAuthorization();
			builder.Services.AddHttpContextAccessor();
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
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
