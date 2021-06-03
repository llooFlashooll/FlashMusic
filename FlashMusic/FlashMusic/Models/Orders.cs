using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Models
{
    public class Orders
    {
        public Guid OrderId { get; set; }
        public int Num { get; set; }
        public int State { get; set; }
        public int UserId { get; set; }
        public Guid HistoryId { get; set; }
    }
}
