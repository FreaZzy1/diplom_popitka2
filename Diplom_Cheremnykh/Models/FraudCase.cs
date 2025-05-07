using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Cheremnykh.Models
{
    public class FraudCase
    {
        [Key]
        public int Id { get; set; }

        
        public int UserId { get; set; }

        
        public User User { get; set; }

        
        public string Description { get; set; }

        public DateTime DetectedAt { get; set; } = DateTime.Now;

        public float FraudProbability { get; set; }
    }
}
