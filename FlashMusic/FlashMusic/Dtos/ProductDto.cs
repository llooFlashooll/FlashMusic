using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Dtos
{
    public class ProductDto
    {
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }

    }

}
