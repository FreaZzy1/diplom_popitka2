using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Cheremnykh.Models
{
    public class FraudInput
    {
        public float Amount { get; set; }
        public float Location { get; set; } // было int
        public float TimeOfDay { get; set; } // было int
    }
}
