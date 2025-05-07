using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Cheremnykh.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        
        public User User { get; set; }

        
        public string Action { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
