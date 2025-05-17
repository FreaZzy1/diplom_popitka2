using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Diplom_Cheremnykh.Models
{
    public class FraudPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsFraud { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
