using Microsoft.Extensions.FileProviders;

namespace HelloApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddCors();
            builder.Services.AddControllers();

            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

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
                app.UseDeveloperExceptionPage();



            app.UseEndpoints(i => i.MapControllers());


            app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}