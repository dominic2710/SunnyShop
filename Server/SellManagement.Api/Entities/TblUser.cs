using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SellManagement.Api.Entities
{
    public class TblUser
    {
        [Key]
        public int Id { get; set; }
        [Column]
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] StoredSalt { get; set; }
        public string UserRole { get; set; }
    }
}
