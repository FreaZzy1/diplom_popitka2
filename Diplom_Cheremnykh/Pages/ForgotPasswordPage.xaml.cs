using System;
using System.Collections.Generic;
using System.Linq;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
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
        
        public ForgotPasswordPage(MainWindow mainWindow, AppDbContext context)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            
        }

        private async void SendCode_Click(object sender, RoutedEventArgs e)
        {
            Msg.Foreground = Brushes.LightCoral;
            Msg.Text = string.Empty;

            var email = EmailTextBox.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(email))
            {
                Msg.Text = "Введите e‑mail.";
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                Msg.Text = "Такой e‑mail не зарегистрирован.";
                return;
            }

            var token = Guid.NewGuid().ToString("N")[..6].ToUpper();   // 6‑значный код
            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            });
            await _context.SaveChangesAsync();

            try
            {
                await SendEmailAsync(email, token);
                Msg.Foreground = Brushes.LightGreen;
                Msg.Text = "Код выслан на почту.";
            }
            catch (Exception ex)
            {
                Msg.Text = $"Не удалось отправить письмо: {ex.Message}";
            }
        }

        /* ---------- 2. сброс пароля ---------- */
        private async void Reset_Click(object sender, RoutedEventArgs e)
        {
            Msg.Foreground = Brushes.LightCoral;
            Msg.Text = string.Empty;

            var email = EmailTextBox.Text.Trim().ToLower();
            var code = CodeTextBox.Text.Trim().ToUpper();
            var pass1 = NewPasswordBox.Password;
            var pass2 = ConfirmBox.Password;

            if (new[] { email, code, pass1, pass2 }.Any(string.IsNullOrWhiteSpace))
            {
                Msg.Text = "Заполните все поля.";
                return;
            }
            if (pass1 != pass2)
            {
                Msg.Text = "Пароли не совпадают.";
                return;
            }
            if (pass1.Length < 6)
            {
                Msg.Text = "Минимум 6 символов в пароле.";
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var token = _context.PasswordResetTokens
                                .FirstOrDefault(t => t.UserId == user.Id &&
                                                     t.Token == code &&
                                                     !t.IsUsed &&
                                                     t.ExpiresAt > DateTime.UtcNow);
            if (token == null)
            {
                Msg.Text = "Неверный или просроченный код.";
                return;
            }

            token.IsUsed = true;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(pass1);
            await _context.SaveChangesAsync();

            Msg.Foreground = Brushes.LightGreen;
            Msg.Text = "Пароль изменён, переход к входу…";
            await Task.Delay(1500);
            _mainWindow.OpenPages(new LoginPage(_mainWindow, _context));
        }

        /* ---------- 3. отправка почты ---------- */
        private static async Task SendEmailAsync(string to, string code)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("noreply@site.com"));
            message.To.Add(MailboxAddress.Parse(to));          // ← адрес получателя
            message.Subject = "Код сброса пароля";
            message.Body = new TextPart("plain")
            { Text = $"Ваш код для сброса пароля: {code}\nКод действует 15 минут." };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("cheremnykh1707@gmail.com", "dtudwxegadifqetl"); 
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}


