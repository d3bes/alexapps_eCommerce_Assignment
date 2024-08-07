using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using project.Core.Dto;
using project.Core.Models;
using project.EF.Repository;
using project.Core.Interfaces;
using System.Security.Claims;
using project.Api.Extensions;

namespace project.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // {
        //   "email": "merchant2@example.com",
        //   "password": "Admin_pwd1"
        // }  

        // {
        //   "email": "admin@example.com",
        //   "password": "Admin_pwd_12345"
        // }
        // private readonly IBaseRepository<Product> _productRepository;
        // private readonly IBaseRepository<Cart> _cartRepository;
        // private readonly IBaseRepository<CartItem> _cartItemRepository;
        // private readonly IBaseRepository<Merchant> _merchantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork)//IBaseRepository<Product> productRepository, IBaseRepository<Cart> cartRepository,IBaseRepository<CartItem> cartItemRepository, IBaseRepository<Merchant> merchantRepository
        {
            // _productRepository = productRepository;
            // _cartItemRepository = cartItemRepository;
            // _cartRepository = cartRepository;
            // _merchantRepository = merchantRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        

        [HttpGet("all/stores")]
        public async Task<IActionResult> GetAllStores()
        {
            // var merchants = await _merchantRepository.getAllAsync(["Products", "user"]);
            var merchants = await _unitOfWork.merchants.getAllAsync(["Products", "user"]);
            return Ok(merchants.ToMerchantStoreListDto());


        }

        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            if (ModelState.IsValid)
            {
                // var product = await _productRepository.getByIdAsync(addToCartDto.ProductId);
                // var product = await _productRepository.findAsync(p => p.Id == addToCartDto.ProductId, ["Merchant"]);
                var product = await _unitOfWork.products.findAsync(p => p.Id == addToCartDto.ProductId, ["Merchant"]);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // var cart = await _cartRepository.findAsync(c => c.UserId == currentUserId, ["Items", "User"]);
                var cart = await _unitOfWork.carts.findAsync(c => c.UserId == currentUserId, ["Items", "User"]);

                if (cart == null)
                {
                    cart = new Cart { UserId = currentUserId, Items = new List<CartItem>() };
                    // await _cartRepository.addAsync(cart);
                    await _unitOfWork.carts.addAsync(cart);
                    _unitOfWork.Complete();
                }

                var cartItem = new CartItem()
                {
                    ProductId = product.Id,
                    Quantity = addToCartDto.Quantity,
                    CartId = cart.Id
                };

                cart.Items.Add(cartItem);
                // await _cartItemRepository.addAsync(cartItem);
                await _unitOfWork.cartItems.addAsync(cartItem);
                _unitOfWork.Complete();

                return Ok(new { Message = "Product added to cart successfully" });
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("Cart")]
        public async Task<IActionResult> GetCart()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var cart = await _cartRepository.findAsync(c => c.UserId == currentUserId, ["Items.Product"]);
            var cart = await _unitOfWork.carts.findAsync(c => c.UserId == currentUserId, ["Items.Product"]);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }
            decimal ItemsVat = 0;
            var cartTotal = cart.Items.Sum(item =>
            {
                var product = item.Product;
                decimal productPrice = product.Price;
                // var merchant = _merchantRepository.getById(product.MerchantId);
                var merchant = _unitOfWork.merchants.getById(product.MerchantId);

                //if vat is Included on product 
                decimal Vat = productPrice * (merchant.VatPercentage ?? 0) / 100;
                if (!merchant.IsVatIncluded)
                {
                    productPrice += Vat;
                    ItemsVat += (Vat * item.Quantity);

                }
                return productPrice * item.Quantity;
            });

            decimal shippingCost = cart.Items.FirstOrDefault()?.Product.Merchant.ShippingCost ?? 0;

            cartTotal += shippingCost;

            return Ok(new
            {
                Cart = cart.Items.Select(item => new
                {
                    item.Id,
                    item.ProductId,
                    item.Product.NameEn,
                    item.Product.NameAr,
                    item.Quantity,
                    Price = item.Product.Price,
                    TotalPrice = item.Product.Price * item.Quantity
                }),

                Vat = ItemsVat,
                ShippingCost = shippingCost,
                CartTotal = cartTotal
            });
        }
        [Authorize]
        [HttpDelete("CartItem/{itemID}")]
        public async Task<IActionResult> DeleteCartItem(int itemID, int productID)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // var cart = await _cartRepository.findAsync(c => c.UserId == currentUserId, ["Items.Product"]);
                // var cartItem = await _cartItemRepository.findAsync(c => c.CartId == cart.Id && c.ProductId == productID);
                // _cartItemRepository.Delete(cartItem);
                 var cart = await _unitOfWork.carts.findAsync(c => c.UserId == currentUserId, ["Items.Product"]);
                var cartItem = await _unitOfWork.cartItems.findAsync(c => c.CartId == cart.Id && c.ProductId == productID);
                _unitOfWork.cartItems.Delete(cartItem);
                _unitOfWork.Complete();
                _logger.LogInformation(message: $"CartItem Id : {cartItem.Id} deleted successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while trying to delete CartItem.");

                return StatusCode(500, $"An error occurred while trying to delete CartItem.");
            }
        }


    }
}