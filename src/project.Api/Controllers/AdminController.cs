using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project.Api.Extensions;
using project.Core.Consts;
using project.Core.Dto;
using project.Core.Models;

namespace project.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<Merchant>  _userManager;
        public AdminController(UserManager<Merchant> userManager)
        {
            _userManager = userManager;   
        }

        [HttpPost("CreateMerchant")]
     public async Task<IActionResult> RegisterMerchant([FromBody] RegisterMerchantDto registerMerchantDto )
     {
        if (ModelState.IsValid)
            {
                var merchant = registerMerchantDto.ToMerchant(); 
                var result = await _userManager.CreateAsync(merchant, registerMerchantDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(merchant, Role.merchant);
                    return Ok(new { Message = "merchant registered successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
     }

    }
}