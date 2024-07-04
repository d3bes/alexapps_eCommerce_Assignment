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
        public static User ToUser(this RegisterDto registerDto)
        {
            return new User(){
            Email = registerDto.


            };
        }
    }
}








