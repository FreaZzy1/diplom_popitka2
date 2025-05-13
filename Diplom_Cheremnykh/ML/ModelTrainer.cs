using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom_Cheremnykh.Models;
using Microsoft.ML;

namespace Diplom_Cheremnykh.ML
{
    class ModelTrainer
    {
        public static void TrainAndSaveModel(string modelPath)
        {
            var mlContext = new MLContext();

            // Обучающие данные
            var trainingData = new List<FraudInput>
{
    new FraudInput { Amount = 100, Location = 1, TimeOfDay = 10 },
    new FraudInput { Amount = 5000, Location = 3, TimeOfDay = 2 },
    new FraudInput { Amount = 150, Location = 2, TimeOfDay = 16 },
    new FraudInput { Amount = 9000, Location = 5, TimeOfDay = 1 },
};

            var labels = new List<bool> { false, true, false, true };

            var trainingList = new List<ModelInput>();
            for (int i = 0; i < trainingData.Count; i++)
            {
                trainingList.Add(new ModelInput
                {
                    Amount = trainingData[i].Amount,
                    Location = trainingData[i].Location,
                    TimeOfDay = trainingData[i].TimeOfDay,
                    IsFraud = labels[i]
                });
            }

            var dataView = mlContext.Data.LoadFromEnumerable(trainingList);

            // Пайплайн для обучения
            var pipeline = mlContext.Transforms.Concatenate("Features", nameof(ModelInput.Amount), nameof(ModelInput.Location), nameof(ModelInput.TimeOfDay))
                .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(ModelInput.IsFraud), featureColumnName: "Features"));

            // Обучаем модель
            var model = pipeline.Fit(dataView);

            // Сохраняем модель в файл
            string modelPath = "ml_model.zip";
            mlContext.Model.Save(model, dataView.Schema, modelPath);

            Console.WriteLine($"Модель сохранена: {modelPath}");

        }

        private class ModelInput : FraudInput
        {
            public bool IsFraud;
        }
    }
}
