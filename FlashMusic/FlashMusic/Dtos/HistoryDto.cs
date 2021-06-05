using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Dtos
{
    // Join History and Product
    public class HistoryGetDto
    {
        public DateTime PayTime { get; set; }
        public int Num { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }

    }
}
