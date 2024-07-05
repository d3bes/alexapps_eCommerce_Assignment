using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using project.Core.filters;

namespace project.Core.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [ArabicCharacters]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string DescriptionEn { get; set; }

        [ArabicCharacters]
        [Required]
        public string DescriptionAr { get; set; }
        [Required]
        public decimal Price { get; set; }


    }
}