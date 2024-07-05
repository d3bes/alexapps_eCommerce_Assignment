using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Dto;
using project.Core.Models;

namespace project.Api.Extensions
{
    public static class ProductExtensions
    {
        public static MerchantProductDto ToMerchantProductDto(this Product product)
        {
            return new MerchantProductDto(){
                DescriptionAr = product.DescriptionAr,
                DescriptionEn = product.DescriptionEn,
                Id = product.Id,
                NameAr  = product.NameAr,
                NameEn = product.NameEn,
                Price = product.Price,
                Quantity = product.Quantity
        
            };
        }

        public static List<MerchantProductDto> ToMerchantProductDtoList(this ICollection<Product> products)
        {
            var productList = new List<MerchantProductDto>();
            foreach (var product in products)
            {
                productList.Add(product.ToMerchantProductDto());
            }
            return productList;
        }
    }
}