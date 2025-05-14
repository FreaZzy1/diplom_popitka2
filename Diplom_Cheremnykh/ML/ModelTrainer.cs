using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom_Cheremnykh.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Diplom_Cheremnykh.ML
{
    class ModelTrainer
    {
        public static void TrainAndSaveModel(string modelPath)
        {
            var mlContext = new MLContext();

            var trainingList = new List<ModelInput>
    {
        new ModelInput { Amount = 100, Location = 1, TimeOfDay = 10, IsFraud = false },
        new ModelInput { Amount = 5000, Location = 3, TimeOfDay = 2, IsFraud = true },
        new ModelInput { Amount = 150, Location = 2, TimeOfDay = 16, IsFraud = false },
        new ModelInput { Amount = 9000, Location = 5, TimeOfDay = 1, IsFraud = true },
    };

            var dataView = mlContext.Data.LoadFromEnumerable(trainingList);

            var pipeline = mlContext.Transforms.Conversion.ConvertType("Amount", outputKind: DataKind.Single)
      .Append(mlContext.Transforms.Conversion.ConvertType("Location", outputKind: DataKind.Single))
      .Append(mlContext.Transforms.Conversion.ConvertType("TimeOfDay", outputKind: DataKind.Single))
      .Append(mlContext.Transforms.Concatenate("Features", "Amount", "Location", "TimeOfDay"))
      .Append(mlContext.BinaryClassification.Trainers.FastForest(labelColumnName: nameof(ModelInput.IsFraud), featureColumnName: "Features"));


            var model = pipeline.Fit(dataView);

            mlContext.Model.Save(model, dataView.Schema, modelPath);
        }

        private class ModelInput : FraudInput
        {
            public bool IsFraud;
        }
    }
}
