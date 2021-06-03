using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Models
{
    [Table("user")]
    public class User
    {
        [Column("userid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("avatar")]
        public string Avatar { get; set; }
    }
}
