using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Cheremnykh.Models
{
    public class PasswordResetToken
    {
        [Key] public int Id { get; set; }

        [Required] public int UserId { get; set; }
        public User User { get; set; }

        [Required] public string Token { get; set; }  // GUID
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
