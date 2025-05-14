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

            // Данные
            var rawData = new List<ModelInput>
            {
                new ModelInput { Amount = 100, Location = 1, TimeOfDay = 10, IsFraud = false },
                new ModelInput { Amount = 5000, Location = 3, TimeOfDay = 2, IsFraud = true },
                new ModelInput { Amount = 150, Location = 2, TimeOfDay = 16, IsFraud = false },
                new ModelInput { Amount = 9000, Location = 5, TimeOfDay = 1, IsFraud = true },
                new ModelInput { Amount = 7500, Location = 4, TimeOfDay = 0, IsFraud = true },
                new ModelInput { Amount = 200, Location = 1, TimeOfDay = 12, IsFraud = false },
                new ModelInput { Amount = 10000, Location = 5, TimeOfDay = 3, IsFraud = true },
                new ModelInput { Amount = 300, Location = 2, TimeOfDay = 14, IsFraud = false },
                new ModelInput { Amount = 8500, Location = 4, TimeOfDay = 4, IsFraud = true },
                new ModelInput { Amount = 180, Location = 1, TimeOfDay = 9, IsFraud = false },
            };

            // Конвертация чисел к float (Single)
            var trainingList = rawData.Select(x => new ModelInput
            {
                Amount = (float)x.Amount,
                Location = (float)x.Location,
                TimeOfDay = (float)x.TimeOfDay,
                IsFraud = x.IsFraud
            }).ToList();

            var dataView = mlContext.Data.LoadFromEnumerable(trainingList);

            // Пайплайн
            var pipeline = mlContext.Transforms.Concatenate("Features", nameof(ModelInput.Amount), nameof(ModelInput.Location), nameof(ModelInput.TimeOfDay))
               .Append(mlContext.BinaryClassification.Trainers.FastForest(
    labelColumnName: "Label", featureColumnName: "Features"));

            // Обучение
            var model = pipeline.Fit(dataView);

            // Сохранение
            mlContext.Model.Save(model, dataView.Schema, modelPath);
            Console.WriteLine("Модель обучена и сохранена.");
        }

        private class ModelInput
        {
            public float Amount { get; set; }
            public float Location { get; set; }
            public float TimeOfDay { get; set; }

            [ColumnName("Label")]
            public bool IsFraud { get; set; }
        }
    }
}
