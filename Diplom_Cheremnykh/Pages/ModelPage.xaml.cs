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
using Microsoft.EntityFrameworkCore;

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для ModelPage.xaml
    /// </summary>
    public partial class ModelPage : Page
    {
        public MainWindow _mainWindow;
        public AppDbContext _context;
        public User _currentUser;
        public ModelPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            _currentUser = currentUser;
            DisplayUserInfo();
        }


        private void DisplayUserInfo()
        {
            try
            {
                string username = _currentUser?.Username;
                var userFromDb = _context.Users.FirstOrDefault(u => u.Username == username);

                if (userFromDb != null)
                {
                    UsernameTextBlock.Text = $"Имя пользователя: {userFromDb.Username}";
                    RoleTextBlock.Text = $"Роль: {userFromDb.Role}";
                }
                else
                {
                    if (_currentUser == null)
                    {
                        MessageBox.Show("Текущий пользователь не определен (null). Возможно, вы не вошли в систему корректно.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных пользователя: {ex.Message}");
            }
        }








        // Обработчик для кнопки управления случаями мошенничества
        private void ManageFraudCasesButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход к странице для управления случаями мошенничества
            _mainWindow.OpenPages(new Pages.FraudCasesPage(_mainWindow, _context,_currentUser));
        }

        // Обработчик для кнопки редактирования профиля
        

        // Обработчик для кнопки выхода из системы
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из системы: возврат на страницу входа
            _mainWindow.OpenPages(new LoginPage(_mainWindow, _context));
        }

        private void CheckFraudButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new PredictionPage(_mainWindow, _context, _currentUser));
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new UsersPage(_mainWindow, _context,_currentUser));
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new DashboardPage(_mainWindow, _context, _currentUser));
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new LogsPage(_mainWindow, _context, _currentUser));
        }
    }
}
