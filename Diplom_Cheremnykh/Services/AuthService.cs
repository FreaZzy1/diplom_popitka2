using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom_Cheremnykh.Data;
using System.Windows;
using Diplom_Cheremnykh.Models;

namespace Diplom_Cheremnykh.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // Регистрация пользователя с хешированием пароля
        public void RegisterUser(string username, string email, string password)
        {
            // Проверяем, есть ли уже такой пользователь
            if (_context.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Пользователь с таким именем уже существует.");
                return;
            }

            // Хешируем пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Создаем нового пользователя
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                IsFraudulent = false
            };

            // Добавляем пользователя в базу данных
            _context.Users.Add(user);
            _context.SaveChanges();

            MessageBox.Show("Пользователь успешно зарегистрирован.");
        }
        

        // Авторизация пользователя с проверкой хешированного пароля
        public bool AuthenticateUser(string username, string password)
        {
            // Получаем пользователя из базы данных по имени
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден.");
                return false; // Пользователь не найден
            }

            // Проверка пароля
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (isPasswordValid)
            {
                MessageBox.Show("Вы успешно вошли в систему!");
                return true;
            }
            else
            {
                MessageBox.Show("Неверный пароль.");
                return false;
            }
        }
    }
}
