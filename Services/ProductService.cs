using HelloApi.Extensions;
using HelloApi.Models;
using HelloApi.Repositories.Interfaces;
using HelloApi.Services.Interfaces;

namespace HelloApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IFileService fileService
            )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task<Product[]> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<int?> GetSellerIdByProductId(int id)
        {
            var product = await _productRepository.GetById(id);
            return (product is null) ? null : product.SellerId;
        }

        public async Task<Product?> GetById(int id)
        {
            var product = await _productRepository.GetById(id);
            return product;
        }

        public async Task<Product> Add(Product product, IFormFile? image = null)
        {
            var newImage = (image is null) ? null : await _fileService.SaveImage(image);
            product.Image = newImage;
            product.Name = product.Name.FirstCharToUpper();
            var newProduct = await _productRepository.Add(product);

            return newProduct;
        }

        public async Task<Product?> Update(Product product, IFormFile? image = null)
        {
            var oldImage = await _productRepository.GetImageById(product.Id);
            var newImage = (image is null) ? null : await _fileService.SaveImage(image);

            product.Image = (newImage is null) ? oldImage : newImage;

            product.Name = product.Name.FirstCharToUpper();

            var updatedProduct = await _productRepository.Update(product);

            if ((newImage is not null) && (updatedProduct is not null))
                _fileService.DeleteImage(oldImage);

            if (updatedProduct is null)
                _fileService.DeleteImage(newImage);

            return updatedProduct;
        }

        public async Task<bool> Delete(int id)
        {
            var deletedProduct = await _productRepository.Delete(id);

            if (deletedProduct is null)
                return false;

            _fileService.DeleteImage(deletedProduct.Image);
            return true;
        }


        public async Task<Category> AddCategory(Category category)
        {
            category.Name = category.Name.FirstCharToUpper();
            var result = await _categoryRepository.Add(category);
            return result;
        }

        public async Task<Category[]> GetAllCategories()
        {
            var result = await _categoryRepository.GetAll();
            return result;
        }
    }
}
