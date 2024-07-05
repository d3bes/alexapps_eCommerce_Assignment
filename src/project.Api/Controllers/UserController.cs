using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using project.Core.Dto;

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
private readonly IBaseRepository<Product> _productRepository ;
public UserController(IBaseRepository<Product> productRepository)
{
    _productRepository = productRepository;
}

        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            if (ModelState.IsValid)
            {
                var product = await _productRepository.getByIdAsync(addToCartDto.ProductId);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = await _cartRepository.findAsync(c => c.UserId == currentUserId);
                if (cart == null)
                {
                    cart = new Cart { UserId = currentUserId, Items = new List<CartItem>() };
                    await _cartRepository.addAsync(cart);
                }

                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = addToCartDto.Quantity,
                    CartId = cart.Id
                };

                cart.Items.Add(cartItem);
                await _cartItemRepository.addAsync(cartItem);

                return Ok(new { Message = "Product added to cart successfully" });
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("Cart")]
        public async Task<IActionResult> GetCart()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartRepository.findAsync(c => c.UserId == currentUserId, ["Items.Product"]);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartTotal = cart.Items.Sum(item =>
            {
                var product = item.Product;
                decimal productPrice = product.Price;
                var merchant = _merchantRepository.getById(int.Parse(product.MerchantId));

                if (!merchant.IsVatIncluded)
                {
                    productPrice += productPrice * (merchant.VatPercentage ?? 0) / 100;
                }

                return productPrice * item.Quantity;
            });

            cartTotal += cart.Items.FirstOrDefault()?.Product.Merchant.ShippingCost ?? 0;

            return Ok(new
            {
                Cart = cart.Items.Select(item => new
                {
                    item.Product.NameEn,
                    item.Quantity,
                    Price = item.Product.Price,
                    TotalPrice = item.Product.Price * item.Quantity
                }),
                CartTotal = cartTotal
            });
        }
    }
}