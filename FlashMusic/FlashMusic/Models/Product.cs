using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Models
{
    [Table("product")]
    public class Product
    {
        [Column("productid")]
        public Guid ProductId { get; set; }
        [Column("categoryid")]
        public int CategoryId { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("picurl")]
        public string PicUrl { get; set; }
        [Column("orderid")]
        public Guid OrderId { get; set; }
        
    }
}
