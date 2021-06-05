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
        // 0未结算，1结算，2超过结算时间
        public int State { get; set; }
    }
}
