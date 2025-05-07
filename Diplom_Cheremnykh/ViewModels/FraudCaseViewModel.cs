using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Cheremnykh.ViewModels
{
    public class FraudCaseViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public string DetectedAt { get; set; }
        public float FraudProbability { get; set; }
    }
}
