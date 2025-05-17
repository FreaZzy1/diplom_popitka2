using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Diplom_Cheremnykh.Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Diplom_Cheremnykh.ML
{
    public class ModelTrainer
    {
        public class ModelInput
        {
            public float Amount { get; set; }
            public float Location { get; set; }
            public float TimeOfDay { get; set; }

            [ColumnName("Label")]
            public bool IsFraud { get; set; }
        }

        public static void TrainAndSaveModel(string modelPath)
        {
            var mlContext = new MLContext(seed: 1);

            // Синтетические данные (100 примеров)
            var data = GenerateFakeData(100);

            // Загрузка в IDataView
            var dataView = mlContext.Data.LoadFromEnumerable(data);

            // Разделение на train/test
            var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            // Пайплайн
            var pipeline = mlContext.Transforms.Concatenate("Features", nameof(ModelInput.Amount), nameof(ModelInput.Location), nameof(ModelInput.TimeOfDay))
.Append(mlContext.BinaryClassification.Trainers.FastTree(
    labelColumnName: "Label",
    featureColumnName: "Features"));

            // Обучение
            var model = pipeline.Fit(split.TrainSet);

            // Оценка
            var predictions = model.Transform(split.TestSet);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions);

            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1: {metrics.F1Score:P2}");

            // Сохранение модели
            mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
            Console.WriteLine($"Модель сохранена в {modelPath}");
        }

        private static List<ModelInput> GenerateFakeData(int count)
        {
            var rand = new Random();
            var data = new List<ModelInput>();

            for (int i = 0; i < count; i++)
            {
                var amount = (float)(rand.Next(50, 10000));
                var location = (float)(rand.Next(1, 6)); // локации от 1 до 5
                var time = (float)(rand.Next(0, 24)); // часы

                bool isFraud = (amount > 5000 && (time < 5 || time > 22)) || (location == 5 && amount > 8000);

                data.Add(new ModelInput
                {
                    Amount = amount,
                    Location = location,
                    TimeOfDay = time,
                    IsFraud = isFraud
                });
            }

            return data;
        }
    }
}
