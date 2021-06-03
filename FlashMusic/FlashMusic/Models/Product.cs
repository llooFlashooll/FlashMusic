using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; }
        
    }
}
