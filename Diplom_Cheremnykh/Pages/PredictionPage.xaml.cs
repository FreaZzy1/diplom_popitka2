﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Diplom_Cheremnykh.Data;
using Diplom_Cheremnykh.Helpers;
using Diplom_Cheremnykh.ML;
using Diplom_Cheremnykh.Models;
using Microsoft.ML;

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для PredictionPage.xaml
    /// </summary>
    public partial class PredictionPage : Page
    {
        public readonly AppDbContext _context;
        public readonly MainWindow _mainWindow;
        public readonly User _currentUser;

        private PredictionEngine<FraudInput, FraudPrediction>? _predictionEngine;
        private bool _modelLoaded = false;
        public PredictionPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _context = context;
            _mainWindow = mainWindow;
            _currentUser = currentUser;

            LoadModel();
        }

        private void LoadModel()
        {
            var mlContext = new MLContext();
            string modelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ml_model.zip");
            ModelTrainer.TrainAndSaveModel(modelPath);


            if (!File.Exists(modelPath))
            {
                MessageBox.Show("Файл модели не найден. Сначала обучите модель.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var trainedModel = mlContext.Model.Load(modelPath, out _);
                _predictionEngine = mlContext.Model
      .CreatePredictionEngine<FraudInput, FraudPrediction>(trainedModel);
                _modelLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке модели: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PredictButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_modelLoaded || _predictionEngine == null)
            {
                PredictionResultTextBlock.Text = "Модель не загружена.";
                return;
            }

            if (!float.TryParse(AmountTextBox.Text, out float amount) ||
                !int.TryParse(LocationTextBox.Text, out int location) ||
                !int.TryParse(TimeOfDayTextBox.Text, out int timeOfDay))
            {
                PredictionResultTextBlock.Text = "Введите корректные числовые значения.";
                return;
            }

            var input = new FraudInput
            {
                Amount = amount,
                Location = location,
                TimeOfDay = timeOfDay
            };

            try
            {
                var result = _predictionEngine.Predict(input);

                var fraudCase = new FraudCase
                {
                    UserId = _currentUser.Id,
                    Description = $"Amount={amount}, Loc={location}, Time={timeOfDay}",
                    DetectedAt = DateTime.UtcNow,
                    FraudProbability = result.Probability
                };

                _context.FraudCases.Add(fraudCase);
                _context.SaveChanges();

                PredictionResultTextBlock.Text = result.IsFraud
                    ? $"⚠ Мошенничество ({result.Probability:P1})"
                    : $"✅ Безопасно ({result.Probability:P1})";

                // ✅ Логирование действия
                Logger.LogAction(
                    _currentUser.Id,
                    _currentUser.Email,
                    $"Сделан прогноз. Сумма: {amount}, Локация: {location}, Время: {timeOfDay}, Результат: {(result.IsFraud ? "Мошенничество" : "Безопасно")}, Вероятность: {result.Probability:P2}"
                );
            }
            catch (Exception ex)
            {
                PredictionResultTextBlock.Text = $"Ошибка при прогнозе: {ex.Message}";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new ModelPage(_mainWindow, _context, _currentUser));
        }
    }
}