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
    /// Логика взаимодействия для ModelPage.xaml
    /// </summary>
    public partial class ModelPage : Page
    {
        public MainWindow _mainWindow;
        private AppDbContext _context;
        public User _currentUser;
        public ModelPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            _currentUser = currentUser;
            DisplayUserInfo();
        }

        // Метод для отображения информации о пользователе
        private void DisplayUserInfo()
        {
            // Проверка, что пользователь существует
            if (_currentUser != null)
            {
                // Установка имени пользователя и роли в текстовые блоки
                var username = _currentUser.Username;
                var role = _currentUser.Role;

                // Обновляем текст на странице
                var usernameTextBlock = (TextBlock)FindName("UsernameTextBlock");
                var roleTextBlock = (TextBlock)FindName("RoleTextBlock");

                usernameTextBlock.Text = $"Имя пользователя: {username}";
                roleTextBlock.Text = $"Роль: {role}";
            }
        }

        // Обработчик для кнопки управления случаями мошенничества
        private void ManageFraudCasesButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход к странице для управления случаями мошенничества
            _mainWindow.OpenPages(new Pages.FraudCasesPage(_mainWindow, _context));
        }

        // Обработчик для кнопки редактирования профиля
        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход к странице для редактирования профиля пользователя
            _mainWindow.OpenPages(new Pages.EditProfilePage(_mainWindow, _context, _currentUser));
        }

        // Обработчик для кнопки выхода из системы
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из системы: возврат на страницу входа
            _mainWindow.OpenPages(new LoginPage(_mainWindow, _context,_currentUser));
        }
    }
}
