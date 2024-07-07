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
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        // private readonly IBaseRepository<Merchant> _merchantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;
        public AdminController(UserManager<User> userManager, ILogger<AccountController> logger, IUnitOfWork unitOfWork) // IBaseRepository<Merchant> merchantRepository,
        {
            _userManager = userManager;
            // _merchantRepository = merchantRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [Authorize(Roles = Role.admin)]
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

                var user = await _userManager.FindByEmailAsync(registerMerchantDto.Email) ?? null;
                if (user == null)
                {
                    _logger.LogInformation(message: $"can't register mercahant due to : user [{registerMerchantDto.Email}] not registered");
                    return BadRequest($" user [ {registerMerchantDto.Email} ] not registered");
                }
                merchant.UserId = user.Id;
                // bool checkMerchant = await _merchantRepository.foundAsync(m => m.UserId == user.Id);
                bool checkMerchant = await _unitOfWork.merchants.foundAsync(m => m.UserId == user.Id);

                if (!checkMerchant)
                {
                    // await _merchantRepository.addAsync(merchant);
                    await _unitOfWork.merchants.addAsync(merchant);
                    _unitOfWork.Complete();
                    await _userManager.AddToRoleAsync(user, Role.merchant);
                    return Ok(new { Message = "merchant registered successfully" });
                }
                else
                {

                    return BadRequest($"Duplicate : User {user.UserName} already registered");
                }
            }

            return BadRequest($"Failed to register merchant");
        }

        [Authorize(Roles = Role.admin)]
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

        [Authorize(Roles = Role.admin)]
        [HttpDelete("{storeID}/Remove")]
        public async Task<IActionResult> RemoveStore(int storeID)
        {
            try
            {
                // var Store = await _merchantRepository.getByIdAsync(storeID);
                var Store = await _unitOfWork.merchants.getByIdAsync(storeID);

                if (Store != null)
                {
                    // _merchantRepository.Delete(Store);
                    _unitOfWork.merchants.Delete(Store);
                    _unitOfWork.Complete();

                    if (Store.user == null)
                    {
                        var user = await _userManager.FindByIdAsync(Store.UserId);
                        await _userManager.RemoveFromRoleAsync(user, Role.merchant);

                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(Store.user, Role.merchant);
                    }

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