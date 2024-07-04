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
        public string FullName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string StoreName { get; set; }
          [Required]
        public decimal ShippingCost { get; set; }

        [Required]
        public bool IsVatIncluded { get; set; }

        public decimal? VatPercentage { get; set; }

          public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (IsVatIncluded && !VatPercentage.HasValue)
            {
                validationResults.Add(new ValidationResult("VatPercentage is required when IsVatIncluded is true.", new[] { nameof(VatPercentage) }));
            }

            return validationResults;
        }
    }

}