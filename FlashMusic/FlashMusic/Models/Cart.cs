using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Models
{
    public class Cart
    {
        public int UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Num { get; set; }
        public int State { get; set; }
    }
}
