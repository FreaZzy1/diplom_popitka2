using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Diplom_Cheremnykh.Data;
using Diplom_Cheremnykh.Models;
using Diplom_Cheremnykh.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Diplom_Cheremnykh.Pages
{
    public partial class FraudCasesPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly AppDbContext _context;
        public readonly User _currentUser;

        public FraudCasesPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;

            LoadFraudCases();
            _currentUser = currentUser;
        }

        private void LoadFraudCases()
        {
            try
            {
                var fraudCases = _context.FraudCases
                    .Include(fc => fc.User) // Загружаем данные из связанной таблицы User
                    .Select(fc => new FraudCaseViewModel
                    {
                        Id = fc.Id,
                        Username = fc.User.Username,
                        Description = fc.Description,
                        DetectedAt = fc.DetectedAt.ToString("g"), // Форматируем дату
                        FraudProbability = fc.FraudProbability
                    })
                    .ToList();

                FraudCasesDataGrid.ItemsSource = fraudCases;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

       

        private void EditFraudCase_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция редактирования пока не реализована.");
        }

        private void DeleteFraudCase_Click(object sender, RoutedEventArgs e)
        {
            if (FraudCasesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            dynamic selected = FraudCasesDataGrid.SelectedItem;
            int caseId = selected.Id;

            var caseToDelete = _context.FraudCases.FirstOrDefault(f => f.Id == caseId);
            if (caseToDelete != null)
            {
                if (MessageBox.Show("Удалить выбранный случай?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.FraudCases.Remove(caseToDelete);
                    _context.SaveChanges();
                    LoadFraudCases();
                }
            }
        }

        private void FraudCasesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.OpenPages(new ModelPage(_mainWindow, _context, _currentUser)); // Замените MainPage на вашу стартовую страницу
            }
        }
    }
}
