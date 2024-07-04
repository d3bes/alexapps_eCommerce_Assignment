using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Core.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; }
    }
}