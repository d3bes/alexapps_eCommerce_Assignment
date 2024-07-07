using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project.Api.Extensions;
using project.Core.Consts;
using project.Core.Dto;
using project.Core.Interfaces;
using project.Core.Models;

namespace project.Api.Controllers
{
    [Authorize(Roles = Role.merchant)]
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        // private readonly IBaseRepository<Merchant> _merchantRepository;
        // private readonly IBaseRepository<Product> _productRepository;
        private readonly ILogger<MerchantController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public MerchantController(IUnitOfWork unitOfWork, UserManager<User> userManager, ILogger<MerchantController> logger)//IBaseRepository<Merchant> merchantRepository, IBaseRepository<Product> productRepository,

        {
            // _merchantRepository = merchantRepository;
            // _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }



        [Authorize(Roles = Role.merchant)]
        [HttpGet("Store")]
        public async Task<IActionResult> GetStore()
        {
            // ClaimsPrincipal currentUserClaims = this.User;
            // var currentUserID = currentUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            // var merchant = await _merchantRepository.findAsync(u => u.UserId == currentUserID, ["Products", "user"]);

            var merchant = await GetCurrentMerchant();
            return Ok(merchant.ToMerchantStoreDto());

        }
        [Authorize(Roles = Role.merchant)]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Product product = productDto.ToProduct();
                    // var checkFound = await _productRepository.findAllAsync(p => p.NameEn == product.NameEn || p.NameAr == product.NameAr);
                    var checkFound = await _unitOfWork.products.findAllAsync(p => p.NameEn == product.NameEn || p.NameAr == product.NameAr);

                    if (checkFound.IsNullOrEmpty())
                    {
                        var merchant = await GetCurrentMerchant();

                        product.MerchantId = merchant.Id;
                        // var result = await _productRepository.addAsync(product);
                        var result = await _unitOfWork.products.addAsync(product);
                        _unitOfWork.Complete();
                        // merchant.Products.Add(result);
                        return Ok(result.ToMerchantProductDto());
                    }
                    else
                        return BadRequest($"Duplicate Product : {product.NameEn}");
                }
                else
                    return BadRequest(ModelState.ValidationState);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return StatusCode(500, "Internal server error");
            }



        }
        [HttpGet("Products")]
        public async Task<IActionResult> GetMerchantProducts()
        {
            // ClaimsPrincipal currentUserClaims = this.User;
            // var currentUserID = currentUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var merchant = await _merchantRepository.findAsync(m => m.UserId == currentUserID, ["Products"]);
            Merchant merchant = await GetCurrentMerchant();
            return Ok(merchant.Products.ToMerchantProductDtoList());

        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateStore(ProductDto productDto)
        {
            Product product = productDto.ToProduct();
            // var result = _productRepository.update(product);
            var result = _unitOfWork.products.update(product);
            _unitOfWork.Complete();
            return Ok(result);

        }

        [Authorize(Roles = Role.merchant)]
        [HttpDelete("Product/{productID}/Remove")]
        public async Task<IActionResult> RemoveProduct(int productID)
        {

            try
            {
                // Product product = _productRepository.getById(productID);
                Product product = _unitOfWork.products.getById(productID);
                if (product != null)
                {
                    // _productRepository.Delete(product);
                    _unitOfWork.products.Delete(product);
                    _unitOfWork.Complete();
                    _logger.LogInformation(message: $" Successfully delete product id: {product.Id} , name:{product.NameEn}\n merchantId: {product.MerchantId}\n");
                    return Ok($" Successfully delete product :{product.NameEn} id: {product.Id}");
                }
                else
                {
                    _logger.LogError(productID, message: $"Not found product id: {product.Id}");
                    return BadRequest($"failed to remove product {product.Id} , name:{product.NameEn} ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to delete the product.");

                return StatusCode(500, "An error occurred while trying to delete the product.");
            }
        }

        [HttpPost("ToggelVat")]
        public async Task<IActionResult> ToggleIsVatIncluded([FromBody] ToggleIsVatIncludedDto toggleIsVatIncludedDto)
        {
            try
            {
                Merchant merchant = await GetCurrentMerchant();
                if (merchant.IsVatIncluded)
                {
                    if (toggleIsVatIncludedDto.VatPercentage != 0)
                    {
                        merchant.IsVatIncluded = false;
                        merchant.VatPercentage = toggleIsVatIncludedDto.VatPercentage;
                        // _merchantRepository.update(merchant);
                        _unitOfWork.merchants.update(merchant);
                        _unitOfWork.Complete();
                        _logger.LogInformation($"updated merchant: {merchant.Id} , IsVatIncluded: {merchant.IsVatIncluded}");
                    }
                    else
                    {
                        return BadRequest("vatPercentage must be have a value");
                    }
                }
                else
                {
                    merchant.IsVatIncluded = true;
                    merchant.VatPercentage = 0;
                    // _merchantRepository.update(merchant);
                    _unitOfWork.merchants.update(merchant);
                    _unitOfWork.Complete();
                    _logger.LogInformation($"updated merchant: {merchant.Id} , IsVatIncluded: {merchant.IsVatIncluded}");
                }
                var result = merchant.ToMerchantDto();
                result.Email = merchant.user.Email;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to Toggle vat.");

                return StatusCode(500, "An error occurred while trying to Toggle vat.");
            }
        }


        [HttpPost("SetShipingCost")]
        public async Task<IActionResult> SetShipingCost(decimal shippingCost)
        {
            try
            {
                Merchant merchant = await GetCurrentMerchant();
                merchant.ShippingCost = shippingCost;
              var result =  _unitOfWork.merchants.update(merchant);
                _unitOfWork.Complete();

                return Ok(result.ToMerchantDto());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying set shiping cost.");

                return StatusCode(500, "An error occurred while trying set shiping cost.");
            }
        }


        private async Task<Merchant> GetCurrentMerchant()
        {
            ClaimsPrincipal currentUserClaims = this.User;
            var currentUserID = currentUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            // var merchant = await _merchantRepository.findAsync(u => u.UserId == currentUserID, ["Products", "user"]);
            var merchant = await _unitOfWork.merchants.findAsync(u => u.UserId == currentUserID, ["Products", "user"]);

            if (merchant == null)
            {
                // merchant = await _merchantRepository.getByIdAsync(currentUserID.ToString());
                merchant = await _unitOfWork.merchants.getByIdAsync(currentUserID.ToString());

            }
            return merchant;
        }


    }
}