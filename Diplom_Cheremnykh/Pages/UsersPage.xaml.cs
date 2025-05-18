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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly AppDbContext _context;
        private readonly User _currentUser;
        public UsersPage(MainWindow mainWindow, AppDbContext context, User currentUser)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
            _currentUser = currentUser;
            LoadUsers();
        }
         private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = _context.Users.ToList();
        }

        /* ---------- Кнопки ---------- */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.OpenPages(new ModelPage(_mainWindow, _context, _mainWindow._currentUser));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selected)
            {
                _mainWindow.OpenPages(new EditUserPage(_mainWindow, _context, selected));
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selected)
            {
                var confirm = MessageBox.Show($"Удалить пользователя {selected.Username}?",
                                              "Подтверждение",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    _context.Users.Remove(selected);
                    _context.SaveChanges();
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }
    }
}
