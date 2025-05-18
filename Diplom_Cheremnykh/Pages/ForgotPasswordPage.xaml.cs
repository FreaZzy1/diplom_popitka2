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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Diplom_Cheremnykh.Data;
using Diplom_Cheremnykh.Models;
using Newtonsoft.Json.Linq;

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для ForgotPasswordPage.xaml
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        private readonly AppDbContext _context;
        private readonly MainWindow _mainWindow;
        private readonly User _user;

        // ====== конструктор ======
        public ForgotPasswordPage(MainWindow mainWindow,
                                  AppDbContext context,
                                  User user,
                                  string initialToken = "")
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            _user = user;

            // автоподстановка кода (опционально)
            CodeTextBox.Text = initialToken ?? string.Empty;
        }

        private async void Reset_Click(object s, RoutedEventArgs e)
        {
            var code = CodeTextBox.Text.Trim();
            var pass1 = NewPasswordBox.Password;
            var pass2 = ConfirmBox.Password;

            if (string.IsNullOrWhiteSpace(code) || pass1 != pass2 || pass1.Length < 6)
            {
                Msg.Text = "Проверьте введённые данные.";
                return;
            }
            var token = _context.PasswordResetTokens
                .SingleOrDefault(t => t.Token == code && t.UserId == _user.Id && !t.IsUsed);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                Msg.Text = "Неверный или просроченный код.";
                return;
            }

            // 1) помечаем токен использованным
            token.IsUsed = true;

            // 2) обновляем пароль (хэшируйте!)
            _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(pass1);
            await _context.SaveChangesAsync();

            Msg.Foreground = Brushes.Green;
            Msg.Text = "Пароль успешно изменён.";

            // перенаправляем на логин
            await Task.Delay(1500);
            _mainWindow.OpenPages(new LoginPage(_mainWindow, _context));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new LoginPage(_mainWindow, _context));
        }
    }
}
    

 
