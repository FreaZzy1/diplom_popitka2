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
using Diplom_Cheremnykh.Services;
using Microsoft.EntityFrameworkCore;

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public MainWindow _mainWindow;
        private AppDbContext _context;
        public User _currentUser;
        public LoginPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            _currentUser = currentUser;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя и пароль.");
                return;
            }

            // Попытка авторизации
            bool loginSuccessful = AuthenticateUser(username, password);

            if (loginSuccessful)
            {
                // Если вход успешен, переход на другую страницу
                MessageBox.Show("Вы успешно вошли в систему.");
                _mainWindow.OpenPages(new Pages.ModelPage(_mainWindow, _context, _currentUser)); // Переход на главную страницу
            }
            else
            {
                // Если вход не успешен
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }

        // Метод для аутентификации пользователя
        private bool AuthenticateUser(string username, string password)
        {
            // Проверка наличия пользователя в базе данных
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return false; // Пользователь не найден
            }

            // Проверка на возможное наличие DBNull в PasswordHash
            string passwordHash = user.PasswordHash as string;  // Преобразуем в строку, если возможно

            if (string.IsNullOrEmpty(passwordHash))
            {
                return false; // Если пароль хэш пуст или NULL, авторизация невозможна
            }

            // Сравниваем введённый пароль с хэшированным паролем
            try
            {
                if (!BCrypt.Net.BCrypt.Verify(password, passwordHash))
                {
                    return false; // Неверный пароль
                }

                // Теперь, когда пользователь авторизован, проверяем его роль
                if (user.Role == "Admin")
                {
                    // Логика для администратора
                    MessageBox.Show("Вы авторизовались как Администратор.");
                    return true;
                }
                else if (user.Role == "User")
                {
                    // Логика для обычного пользователя
                    MessageBox.Show("Вы авторизовались как пользователь.");
                    return true;
                }
                else
                {
                    MessageBox.Show("Неизвестная роль.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке пароля: {ex.Message}");
                return false;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу регистрации
            _mainWindow.OpenPages(new Pages.RegisterPage(_mainWindow, _context, _currentUser));
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика восстановления пароля (можно добавить в будущем)
            MessageBox.Show("Функция восстановления пароля пока не доступна.");
        }
    }
}
