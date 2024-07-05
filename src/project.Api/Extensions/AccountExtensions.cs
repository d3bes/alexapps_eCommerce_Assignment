using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Models;
using project.Core.Dto;


namespace project.Api.Extensions
{
    public static class AccountExtensions
    {
        public static string ToUserName(this string fullName)
        {
            var rand = new Random();
            var random = rand.Next(1,999).ToString();
           var full_Name= fullName.Replace(" ", "_").ToLower();
            string UserName = $"{full_Name}_{random}";
            return UserName;
        }
        public static User ToUser(this RegisterDto registerDto)
        {
            return new User(){
            Email = registerDto.Email,
            FullName = registerDto.FullName,
            UserName = registerDto.FullName.ToUserName()
            };
        }

        public static UserDto ToUserDto(this User user)
        {
            return new UserDto(){
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                phoneNumber = user.PhoneNumber,
                UserName = user.FullName
            };

        }
    }
}








