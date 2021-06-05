using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Dtos
{
    // Join Cart and Product
    public class CartGetDto
    {
        public int Num { get; set; }
        public int State { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }
    }

    public class CartAddDto
    {
        public int Num { get; set; }
        public string Name { get; set; }
    }

    public class CartDelDto
    {
        public string Name { get; set; }
    }

    public class CartUpdateDto
    {
        public int Num { get; set; }
        public string Name { get; set; }
    }
}
