using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace project.Core.Models
{
    public class Merchant 
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public bool IsVatIncluded { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal? VatPercentage { get; set; }
        
        public string UserId { get; set; }
        public User user { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}