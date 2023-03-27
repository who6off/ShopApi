using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopApi.Authentication;
using ShopApi.Authentication.Interfaces;
using ShopApi.Authorization;
using ShopApi.Configuration;
using ShopApi.Data;
using ShopApi.Data.Repositories;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Helpers;
using ShopApi.Middleware;
using ShopApi.Repositories;
using ShopApi.Services;
using ShopApi.Services.Interfaces;
using System.Text;

namespace ShopApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder();
			var (services, configuration) = (builder.Services, builder.Configuration);


			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				 .AddJwtBearer(options =>
				 {
					 options.RequireHttpsMetadata = false;
					 options.SaveToken = true;
					 var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
					 options.TokenValidationParameters = new TokenValidationParameters()
					 {
						 ValidateIssuer = true,
						 ValidateAudience = true,
						 ValidateLifetime = true,
						 ValidateActor = true,
						 ValidAudience = jwtSettings.Audience,
						 ValidIssuer = jwtSettings.Issuer,
						 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
					 };
				 });

			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					AgeRestrictionPolicy.Name,
					p => p.AddRequirements(new AgeRestrictionPolicy(configuration.GetAdultAge())));
			});

			services.AddCors();
			services.AddControllers();

			services.AddDbContext<ShopContext>(options =>
			{
				options.UseSqlServer(
					configuration.GetConnectionString(builder.Environment.EnvironmentName));
			});

			services.AddAutoMapper(typeof(Program));

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopAPI", Version = "v1" });

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme()
						{
							Reference = new OpenApiReference()
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
					new string[] {}
				}});
			});

			services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
			services.Configure<ShopDbSettings>(configuration.GetSection(ShopDbSettings.SectionName));


			services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

			services.AddSingleton<IAuthorizationHandler, AgeRestrictionPolicyHandler>();
			services.AddTransient<IAuthorizationHandler, ProductAccessRestrictions>();
			services.AddTransient<IAuthorizationHandler, OrderAccessRestrictions>();

			services.AddSingleton<IPasswordHasher, PasswordHasher>();
			services.AddSingleton<ITokenGenerator, TokenGenerator>();
			services.AddSingleton<IValidator, Validator>();
			services.AddSingleton<IFileService, FileService>();

			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IOrderService, OrderService>();

			var app = builder.Build();

			app.UseExceptionHandlingMiddleware();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseCors(i => i.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(Directory.GetCurrentDirectory(),
					@$"wwwroot/{configuration.GetValue<string>("ImagesFolder")}")),
				RequestPath = new PathString($"/{configuration.GetValue<string>("ImagesFolder")}")
			});

			app.UseEndpoints(i => i.MapControllers());

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopAPI V1");

			});

			app.MapGet("/", (HttpContext context) =>
			{
				var swaggerHref = context.Request.GetDisplayUrl() + "swagger";

				context.Response.ContentType = "text/html";
				return @$"
					<p>API is launched</p>
					<p><a href='{swaggerHref}'>Open Swagger UI</a></p>
				";
			});

			ServiceAccessor.Services = app.Services;

			app.Run();
		}
	}
}