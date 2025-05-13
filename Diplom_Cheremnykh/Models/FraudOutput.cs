using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Diplom_Cheremnykh.Models
{
    public class FraudOutput
    {
        [ColumnName("PredictedLabel")]
        public bool IsFraud;

        public float Score;
    }

}
