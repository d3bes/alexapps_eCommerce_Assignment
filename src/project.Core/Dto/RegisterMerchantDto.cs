using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.Core.Dto
{
    public class RegisterMerchantDto
    {
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string StoreName { get; set; }
          [Required]
        public decimal ShippingCost { get; set; }

        [Required]
        public bool IsVatIncluded { get; set; }

        public decimal? VatPercentage { get; set; }

    }

}