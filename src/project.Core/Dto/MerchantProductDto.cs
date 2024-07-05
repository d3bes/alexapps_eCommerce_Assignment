using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Core.Dto
{
    public class MerchantProductDto
    {

        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public decimal Price { get; set; }
    }
}