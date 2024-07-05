using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project.Api.Extensions;
using project.Core.Consts;
using project.Core.Dto;
using project.Core.Interfaces;
using project.Core.Models;
using project.Service.Contracts;

namespace project.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;
        private readonly IBaseRepository<User> _userRepository;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
          ITokenService tokenService, ILogger<AccountController> logger,IBaseRepository<User> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _userRepository.findAllAsync(u => u.Email == registerDto.Email);
                if (checkUser.IsNullOrEmpty())
                {
                    var user = registerDto.ToUser();
                    var result = await _userManager.CreateAsync(user, registerDto.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Role.user);
                        _logger.LogInformation($"User : {user.UserName} registered successfully");
                        return Ok(new { Message = "User registered successfully" });
                    }

                    _logger.LogError($"User : {user.UserName} failed to register");
                    return BadRequest(result.Errors);
                }
                else
                {
                    return BadRequest($"Duplicate user : {registerDto.Email}");
                }
            }

            return BadRequest(ModelState.ValidationState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(loginDto.Email);
                var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // var token = GenerateJwtToken(appUser);
                    var token = _tokenService.GenerateJwtToken(appUser);
                    var roles = await _userManager.GetRolesAsync(appUser);
                    _logger.LogInformation($"User : {appUser.UserName} login successfully");
                    return Ok(new { Email = loginDto.Email, UserName = appUser.UserName, Token = token, role = roles });
                }

                return Unauthorized(new { Message = "Invalid login attempt" });
            }

            return BadRequest(ModelState.ValidationState);
        }


    }
}