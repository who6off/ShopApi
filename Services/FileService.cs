using HelloApi.Services.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

namespace HelloApi.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public FileService(
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public bool IsImage(IFormFile image)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(image.FileName, out var contentType);
            return contentType.StartsWith("image/");
        }

        public async Task<string?> SaveImage(IFormFile image)
        {
            if (!IsImage(image)) return null;

            var dirPath = $"{_environment.WebRootPath}/{_configuration.GetValue<string>("ImagesFolder")}/";
            var newName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var path = dirPath + newName;

            using (var fs = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }

            return File.Exists(path) ? newName : null;
        }
    }
}
