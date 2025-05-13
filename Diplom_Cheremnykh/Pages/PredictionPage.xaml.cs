using System;
using System.Collections.Generic;
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
        public readonly PredictionEngine<FraudInput, FraudPrediction> _predictionEngine;
        public PredictionPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _context = context;
            _mainWindow = mainWindow;
            _currentUser = currentUser;

            // Настройка ML.NET
            var mlContext = new MLContext();
            ITransformer trainedModel = mlContext.Model.Load("ml_model.zip", out _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<FraudInput, FraudPrediction>(trainedModel);
        }

        private void PredictButton_Click(object sender, RoutedEventArgs e)
        {
            // Пример ввода: данные можно получить из текстбоксов
            var input = new FraudInput
            {
                Amount = float.Parse(AmountTextBox.Text), // Конвертируем Amount в float
                Location = int.Parse(LocationTextBox.Text), // Конвертируем Location в int
                TimeOfDay = int.Parse(TimeOfDayTextBox.Text)
            };

            var prediction = _predictionEngine.Predict(input);

            PredictionResultTextBlock.Text = prediction.IsFraud ?
                "⚠ Обнаружено мошенничество!" :
                "✅ Операция безопасна.";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new ModelPage(_mainWindow, _context, _currentUser));
        }
    }
}
