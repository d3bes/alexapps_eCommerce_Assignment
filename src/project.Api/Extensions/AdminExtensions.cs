using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Dto;
using project.Core.Models;


namespace project.Api.Extensions
{
    public static class AdminExtensions
    {
        public static RegisterMerchantDto ToMerchantDto(this Merchant merchant)
        {
            return new RegisterMerchantDto(){
                IsVatIncluded  = merchant.IsVatIncluded,
                ShippingCost = merchant.ShippingCost,
                StoreName = merchant.StoreName,
                VatPercentage = merchant.VatPercentage,
            
            };
        }

         public static Merchant ToMerchant(this RegisterMerchantDto merchantRegisterDto)
        {
            return new Merchant(){
                IsVatIncluded  = merchantRegisterDto.IsVatIncluded,
                ShippingCost = merchantRegisterDto.ShippingCost,
                StoreName = merchantRegisterDto.StoreName,
                VatPercentage = (decimal)merchantRegisterDto.VatPercentage
                

            
            };
        }
    }
}