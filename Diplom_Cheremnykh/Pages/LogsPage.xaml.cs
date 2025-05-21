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
using ClosedXML.Excel;
using Diplom_Cheremnykh.Data;
using Diplom_Cheremnykh.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogsPage.xaml
    /// </summary>
    public partial class LogsPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        private List<Log> _allLogs;
        public LogsPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _context = context;
            _currentUser = currentUser;
            _mainWindow = mainWindow;
            LoadData();
        }
        private void LoadData()
        {
            _allLogs = _context.Logs
                               .Select(log => new Log
                               {
                                   Id = log.Id,
                                   Action = log.Action,
                                   CreatedAt = log.CreatedAt,
                                   User = log.User,
                                   UserEmail = log.User.Email
                               })
                               .ToList();

            UserFilterComboBox.ItemsSource = _context.Users.ToList();
            LogsGrid.ItemsSource = _allLogs;
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _allLogs.AsQueryable();

            if (UserFilterComboBox.SelectedValue is int userId)
                filtered = filtered.Where(l => l.User.Id == userId);

            if (FromDatePicker.SelectedDate.HasValue)
                filtered = filtered.Where(l => l.CreatedAt >= FromDatePicker.SelectedDate.Value);

            if (ToDatePicker.SelectedDate.HasValue)
                filtered = filtered.Where(l => l.CreatedAt <= ToDatePicker.SelectedDate.Value.AddDays(1));

            var searchText = SearchTextBox.Text?.ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
                filtered = filtered.Where(l =>
                    l.Action.ToLower().Contains(searchText) ||
                    l.User.Email.ToLower().Contains(searchText));

            LogsGrid.ItemsSource = filtered.ToList();
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = "logs_export.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Logs");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Пользователь";
                worksheet.Cell(1, 3).Value = "Событие";
                worksheet.Cell(1, 4).Value = "Дата и время";

                var logs = LogsGrid.ItemsSource.Cast<Log>().ToList();
                for (int i = 0; i < logs.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = logs[i].Id;
                    worksheet.Cell(i + 2, 2).Value = logs[i].UserEmail;
                    worksheet.Cell(i + 2, 3).Value = logs[i].Action;
                    worksheet.Cell(i + 2, 4).Value = logs[i].CreatedAt.ToString("g");
                }

                workbook.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Экспорт завершён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new ModelPage(_mainWindow, _context, _currentUser));
        }
    }
}
