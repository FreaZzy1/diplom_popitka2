using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Cheremnykh.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        
        public string Username { get; set; }

        
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsFraudulent { get; set; } = false;
        public string Role { get; set; }

        public ICollection<Log> Logs { get; set; }
        public ICollection<FraudCase> FraudCases { get; set; }
    }
}
