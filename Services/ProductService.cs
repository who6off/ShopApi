using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Extensions;
using ShopApi.Helpers.Exceptions;
using ShopApi.Models.DTOs.Product;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
	public class ProductService : IProductService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductRepository _productRepository;
		private readonly IFileService _fileService;
		private readonly HttpContext? _httpContext;
		private readonly IMapper _mapper;

		public ProductService(
			ICategoryRepository categoryRepository,
			IProductRepository productRepository,
			IFileService fileService,
			IHttpContextAccessor httpContextAccessor,
			IMapper mapper
			)
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
			_fileService = fileService;
			_httpContext = httpContextAccessor.HttpContext;
			_mapper = mapper;
		}

		public async Task<Product[]> GetAll()
		{
			var products = await _productRepository.GetAll()
				.Where(i => !i.Category.IsForAdults)
				.OrderByDescending(i => i.Id)
				.ToArrayAsync();

			return products;
		}


		public async Task<Product[]> GetByCategory(int categoryId)
		{
			var products = await _productRepository.GetAll()
			   .Where(i => i.CategoryId == categoryId)
			   .OrderByDescending(i => i.Id)
			   .ToArrayAsync();

			return products;
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

		public async Task<ProductDTO?> Add(ProductForCreationDTO dto)
		{
			var product = _mapper.Map<Product>(dto);
			product.Name = product.Name.FirstCharToUpper();
			product.SellerId = _httpContext.User.GetUserId().Value;

			var newImage = (dto.Image is null) ? null : await _fileService.SaveImage(dto.Image);
			product.Image = newImage;

			var newProduct = await _productRepository.Add(product);

			if (newProduct is null)
			{
				throw new AppException("Creation error!");
			}

			var productDTO = _mapper.Map<ProductDTO>(newProduct);
			return productDTO;
		}

		public async Task<ProductDTO?> Update(int id, ProductForUpdateDTO dto)
		{
			var product = await _productRepository.GetById(id);

			if (product is null)
			{
				throw new NotFoundException("This product is not found!");
			}

			var oldImage = product.Image;

			_mapper.Map(dto, product);

			var newImage = (dto.Image is null) ? null : await _fileService.SaveImage(dto.Image);
			product.Image = (newImage is null) ? oldImage : newImage;

			product.Name = product.Name.FirstCharToUpper();
			product.SellerId = _httpContext.User.GetUserId().Value;

			var updatedProduct = await _productRepository.Update(product);

			if ((newImage is not null) && (updatedProduct is not null))
			{
				_fileService.DeleteImage(oldImage);
			}

			if (updatedProduct is null)
			{
				if (newImage is not null)
				{
					_fileService.DeleteImage(newImage);
				}

				throw new AppException("Update error!");
			}

			var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
			return updatedProductDTO;
		}

		public async Task<bool> Delete(int id)
		{
			var deletedProduct = await _productRepository.Delete(id);

			if (deletedProduct is null)
				return false;

			_fileService.DeleteImage(deletedProduct.Image);
			return true;
		}


		public async Task<Category?> GetCategoryById(int id)
		{
			var result = await _categoryRepository.GetById(id);
			return result;
		}


		public async Task<Category> AddCategory(Category category)
		{
			category.Name = category.Name.FirstCharToUpper();
			var result = await _categoryRepository.Add(category);
			return result;
		}


		public async Task<Category[]> GetCategories(bool isAdults)
		{
			throw new NotImplementedException();
			//var result = await _categoryRepository.GetAll()
			//	.Where(i => i.IsForAdults == isAdults)
			//	.ToArrayAsync();
			//return result;
		}

	}
}
