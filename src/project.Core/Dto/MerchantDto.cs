using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Models;

namespace project.Core.Dto
{
    public class MerchantDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public bool IsVatIncluded { get; set; }
        public string Email { get; set; }

        public decimal ShippingCost { get; set; }
        public decimal? VatPercentage { get; set; }

        public UserDto user { get; set; }
         public List<MerchantProductDto> products { get; set; }



    }
}