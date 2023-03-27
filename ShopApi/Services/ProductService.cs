using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ShopApi.Authentication;
using ShopApi.Authorization;
using ShopApi.Data.Models;
using ShopApi.Data.Models.SearchParameters;
using ShopApi.Data.Repositories.Interfaces;
using ShopApi.Extensions;
using ShopApi.Helpers.Exceptions;
using ShopApi.Helpers.Interfaces;
using ShopApi.Models.DTOs.Product;
using ShopApi.Services.Interfaces;

namespace ShopApi.Services
{
	public class ProductService : IProductService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _userRepository;
		private readonly IFileService _fileService;
		private readonly IAuthorizationService _authorizationService;
		private readonly HttpContext? _httpContext;
		private readonly IMapper _mapper;


		public ProductService(
			ICategoryRepository categoryRepository,
			IProductRepository productRepository,
			IUserRepository userRepository,
			IFileService fileService,
			IHttpContextAccessor httpContextAccessor,
			IMapper mapper,
			IAuthorizationService authorizationService
			)
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
			_userRepository = userRepository;
			_fileService = fileService;
			_authorizationService = authorizationService;
			_httpContext = httpContextAccessor.HttpContext;
			_mapper = mapper;
		}


		public async Task<IPageData<ProductDTO>> Get(ProductSearchParameters searchParameters)
		{
			var user = _httpContext.User;

			if (!user.Identity.IsAuthenticated || !user.IsAdult())
			{
				if (searchParameters.Categories is not null)
				{
					foreach (var categoryId in searchParameters.Categories)
					{
						var category = await _categoryRepository.GetById(categoryId);

						if (category.IsForAdults)
						{
							throw new AccessDeniedException("Access denied!");
						}
					}
				}

				if ((searchParameters.IsForAdults is not null) && searchParameters.IsForAdults.Value)
				{
					throw new AccessDeniedException("Access denied!");
				}

				searchParameters.IsForAdults = false;
			}

			var result = await _productRepository.Get(searchParameters);
			var mapResult = result.Map<ProductDTO>(_mapper);
			return mapResult;
		}


		public async Task<ProductDTO> GetById(int id)
		{
			var product = await _productRepository.GetById(id);
			await Authorize(product, ProductOperations.Get);

			var productDTO = _mapper.Map<ProductDTO>(product);
			return productDTO;
		}


		public async Task<ProductDTO> Add(ProductForCreationDTO dto)
		{
			var product = _mapper.Map<Product>(dto);
			await MapProductOperationDTO(product, dto);

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


		public async Task<ProductDTO> Update(int id, ProductForUpdateDTO dto)
		{
			var product = await _productRepository.GetById(id);
			await Authorize(product, ProductOperations.Update);

			var oldImage = product.Image;

			_mapper.Map(dto, product);
			await MapProductOperationDTO(product, dto);

			var newImage = (dto.Image is null) ? null : await _fileService.SaveImage(dto.Image);
			product.Image = (newImage is null) ? oldImage : newImage;

			var updatedProduct = await _productRepository.Update(product);

			if ((newImage is not null) && (updatedProduct is not null))
			{
				_fileService.DeleteImage(oldImage);
			}

			if (updatedProduct is null)
			{
				_fileService.DeleteImage(newImage);

				throw new AppException("Update error!");
			}

			var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);
			return updatedProductDTO;
		}


		public async Task<ProductDTO> Delete(int id)
		{
			var product = await _productRepository.GetById(id);
			await Authorize(product, ProductOperations.Delete);

			var deletedProduct = await _productRepository.Delete(id);

			if (deletedProduct is null)
			{
				throw new AppException("Delete error!");
			}
			else
			{
				_fileService.DeleteImage(deletedProduct.Image);
			}

			var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);
			return deletedProductDTO;
		}


		private async Task MapProductOperationDTO(Product product, ProductOperationDTO dto)
		{
			var userRole = _httpContext.User.GetUserRole();

			if (userRole == UserRoles.Admin)
			{
				if (dto.SellerId is null)
				{
					throw new AppException("SellerId is required!");
				}


				var sellerRole = await _userRepository.GetUserRoleNameById(dto.SellerId.Value);
				if ((sellerRole is null) || (sellerRole != UserRoles.Seller))
				{
					throw new AppException("Incorrect SellerId!");
				}

				product.SellerId = dto.SellerId.Value;
			}

			if (userRole == UserRoles.Seller)
			{
				var userId = _httpContext.User.GetUserId();

				if ((dto.SellerId is not null) && (userId != dto.SellerId))
				{
					throw new AccessDeniedException("SellerId can't be changed");
				}

				product.SellerId = userId.Value;
			}

			product.Name = dto.Name.FirstCharToUpper();
		}


		private async Task Authorize(Product? product, IAuthorizationRequirement requirement)
		{
			if (product is null)
			{
				throw new NotFoundException("Product is not found!");
			}

			var isAuthorized = await _authorizationService.AuthorizeAsync(_httpContext.User, product, requirement);

			if (!isAuthorized.Succeeded)
			{
				throw new AccessDeniedException("Access denied!");
			}
		}
	}
}
