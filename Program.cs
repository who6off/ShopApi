using HelloApi.Authentication;
using HelloApi.Authorization;
using HelloApi.Configuration;
using HelloApi.Data;
using HelloApi.Repositories;
using HelloApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HelloApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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
                    p => p.AddRequirements(new AgeRestrictionPolicy(configuration.GetValue<int>("AdultAge"))));
            });

            services.AddCors();
            services.AddControllers();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<ShopDbSettings>(configuration.GetSection(ShopDbSettings.SectionName));

            services.AddDbContext<ShopContext>(options =>
             {
                 options.UseSqlServer(
                     configuration.GetConnectionString(builder.Environment.EnvironmentName));
             });

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IAuthorizationHandler, AgeRestrictionPolicyHandler>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IValidator, Validator>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();

            var app = builder.Build();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(i => i.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/images")
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/api/error/dev");
            }
            else
            {
                app.UseExceptionHandler("/api/error/prod");
            }

            app.UseEndpoints(i => i.MapControllers());


            app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}