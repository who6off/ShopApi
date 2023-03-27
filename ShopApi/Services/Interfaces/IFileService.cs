namespace ShopApi.Services.Interfaces
{
    public interface IFileService
    {
        public bool IsImage(IFormFile image);
        public Task<string?> SaveImage(IFormFile image);

        public Task<string?> ReplaceImage(IFormFile image, string oldImgeName);

        public Task<bool> DeleteImage(string fileName);
    }
}
