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

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {

        private readonly MainWindow _mainWindow;
        private readonly AppDbContext _context;
        private readonly User _currentUser;

        public DashboardPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            _context = context;
            _currentUser = currentUser;

            LoadStats();
        }

        private void LoadStats()
        {
            // Кол‑во пользователей
            var usersCount = _context.Users.Count();
            UsersCountText.Text = usersCount.ToString();

            // Кол‑во всех проверок
            var checksCount = _context.FraudCases.Count();
            ChecksCountText.Text = checksCount.ToString();

            // Процент мошенничества
            if (checksCount > 0)
            {
                var fraudCount = _context.FraudCases.Count(c => c.FraudProbability >= 0.5f);
                var rate = (float)fraudCount / checksCount * 100f;
                FraudRateText.Text = $"{rate:F1}";
            }
            else
            {
                FraudRateText.Text = "—";
            }

            // Последняя проверка
            var last = _context.FraudCases
                               .OrderByDescending(c => c.DetectedAt)
                               .FirstOrDefault();
            LastCheckText.Text = last != null
                ? $"{last.DetectedAt:g} — {(last.FraudProbability >= 0.5f ? "⚠️" : "✅")}"
                : "—";
        }

        /* ---------- navigation ---------- */
        private void FraudCasesBtn_Click(object sender, RoutedEventArgs e) =>
            _mainWindow.OpenPages(new FraudCasesPage(_mainWindow, _context, _currentUser));

        private void UsersBtn_Click(object sender, RoutedEventArgs e) =>
            _mainWindow.OpenPages(new UsersPage(_mainWindow, _context, _currentUser));

        private void BackBtn_Click(object sender, RoutedEventArgs e) =>
            _mainWindow.OpenPages(new ModelPage(_mainWindow, _context, _currentUser));
    }
}
    
