using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project.Api.Extensions;
using project.Core.Consts;
using project.Core.Dto;
using project.Core.Interfaces;
using project.Core.Models;

namespace project.Api.Controllers
{
    [Authorize(Roles = Role.admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<Merchant> _merchantRepository;
        private readonly ILogger<AccountController> _logger;
        public AdminController(UserManager<User> userManager, IBaseRepository<Merchant> merchantRepository,ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _merchantRepository = merchantRepository;
            _logger = logger;
        }

        [HttpGet("all/stores")]
        public async Task<IActionResult> GetAllStores()
        {
            var merchants = await _merchantRepository.getAllAsync(["Products", "user"]);

            return Ok(merchants.ToMerchantStoreListDto());


        }

        [HttpPost("CreateMerchant")]
        public async Task<IActionResult> RegisterMerchant([FromBody] RegisterMerchantDto registerMerchantDto)
        {
            if (registerMerchantDto.IsVatIncluded)
            {
                registerMerchantDto.VatPercentage = 0;
            }
            if (ModelState.IsValid)
            {
                var merchant = registerMerchantDto.ToMerchant();

                var user = await _userManager.FindByEmailAsync(registerMerchantDto.Email);
                merchant.UserId = user.Id;
                bool checkMerchant = await _merchantRepository.foundAsync(m => m.UserId == user.Id);
                if (!checkMerchant)
                {
                    await _merchantRepository.addAsync(merchant);
                    await _userManager.AddToRoleAsync(user, Role.merchant);
                    return Ok(new { Message = "merchant registered successfully" });
                }
                else
                {

                    return BadRequest($"Duplicate : User {user.UserName} already registered");
                }
            }

            return BadRequest();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAdmin(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = registerDto.ToUser();
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role.admin);
                    return Ok(new { Message = "Admin registered successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState.ValidationState);

        }

        [HttpDelete("{storeID}/Remove")]
        public async Task<IActionResult> RemoveStore(int storeID)
        {
            try
            {
                var Store = await _merchantRepository.getByIdAsync(storeID);
                if (Store != null)
                {
                    _merchantRepository.Delete(Store);
                    _logger.LogInformation(message: $" Successfully delete product id: {Store.Id} , name:{Store.StoreName}\n");
                    return Ok($" Successfully delete Store :{Store.StoreName}");

                }
                else
                {
                    _logger.LogError(Store.Id, message: $"Not found Store : {Store.StoreName}");
                    return BadRequest($"failed to remove product {Store.Id} , name:{Store.StoreName} ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to delete the Store.");

                return StatusCode(500, "An error occurred while trying to delete the Store.");
            }

        }








    }
}