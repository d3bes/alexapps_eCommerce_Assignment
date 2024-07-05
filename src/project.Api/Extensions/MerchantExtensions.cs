using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Dto;
using project.Core.Models;

namespace project.Api.Extensions
{
    public static class MerchantExtensions
    {
        
        public static MerchantDto ToMerchantStoreDto(this Merchant merchant)
        {
            return new MerchantDto(){
                Id = merchant.Id,
                IsVatIncluded = merchant.IsVatIncluded,
                ShippingCost = merchant.ShippingCost,
                StoreName   = merchant.StoreName,
                VatPercentage = merchant.VatPercentage,
                user = merchant.user.ToUserDto(),
                products = merchant.Products.ToMerchantProductDtoList()

            };
        }
        public static List<MerchantDto> ToMerchantStoreListDto(this List<Merchant> merchants)
        {
            List<MerchantDto> result = new List<MerchantDto>(){};
            foreach(var merchant in merchants)
            {
                result.Add(merchant.ToMerchantStoreDto());
            }
            return result;
        }

        public static Product ToProduct(this ProductDto productDto)
        {
            return new Product (){

                DescriptionAr= productDto.DescriptionAr,
                DescriptionEn = productDto.DescriptionEn,
                NameAr = productDto.NameAr,
                NameEn = productDto.NameEn,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };
        }

        // public static ProductDto ToProductDto(this Product product)
        // {
        //     return new ProductDto(){
        //         DescriptionAr = product.DescriptionAr,
        //         DescriptionEn = product.DescriptionEn,
        //         Id = product.Id,
        //         NameAr = product.NameAr,
        //         NameEn = product.NameEn,
        //         Price =   product.Price
        //     };
        // }

        // public static List<ProductDto> ToProductDtoList(this List<Product> productList) 
        // {
        //     List<ProductDto> productDtoList = new List<ProductDto>();
        //     foreach (var product in productList)
        //     {
        //         productDtoList.Add(product.ToProductDto());
        //     }
        //     return productDtoList;
        // }
    }
}