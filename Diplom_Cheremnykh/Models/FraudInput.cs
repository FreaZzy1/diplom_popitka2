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
        public int Location { get; set; }
        public int TimeOfDay { get; set; }
    }
}
