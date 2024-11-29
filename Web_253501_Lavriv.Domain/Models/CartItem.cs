using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_253501_Lavriv.Domain.Entities;

namespace Web_253501_Lavriv.Domain.Models
{
    public class CartItem
    {
        public Detail Item { get; set; } = null!;
        public int Count { get; set; }
    }
}
