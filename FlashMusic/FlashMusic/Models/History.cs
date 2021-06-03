using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Models
{
    [Table("history")]
    public class History
    {
        [Column("historyid")]
        public Guid HistoryId { get; set; }
        [Column("userid")]
        public int UserId { get; set; }
        [Column("orderid")]
        public Guid OrderId { get; set; }
    }
}
