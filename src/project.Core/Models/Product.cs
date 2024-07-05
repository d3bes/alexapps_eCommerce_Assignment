using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
    }
}