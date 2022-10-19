namespace HelloApi.Services.Interfaces
{
    public interface IFileService
    {
        public bool IsImage(IFormFile image);
        public Task<string?> SaveImage(IFormFile image);
    }
}
