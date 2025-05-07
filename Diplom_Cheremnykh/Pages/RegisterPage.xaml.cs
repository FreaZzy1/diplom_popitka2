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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public AppDbContext _context;
        public MainWindow _mainWindow;
        public User _currentUser;
        public RegisterPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            _currentUser = currentUser;
        }

        // Обработчик кнопки регистрации
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            // Проверка, что пароли совпадают
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            // Проверка, есть ли пользователь с таким логином
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким именем или email уже существует.");
                return;
            }

            // Хэширование пароля с помощью BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Создание нового пользователя
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword
            };

            // Сохранение пользователя в базе данных
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно.");
                // Навигация на страницу входа (например, можно закрыть текущую страницу или перейти на LoginPage)
                NavigationService.Navigate(new LoginPage(_mainWindow,_context,_currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при регистрации: {ex.Message}");
            }
        }

        // Обработчик кнопки для возврата на страницу входа
        private void LoginRedirectButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage(_mainWindow, _context, _currentUser));
        }
        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                UsernamePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            EmailPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                EmailPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                ConfirmPasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}
    
