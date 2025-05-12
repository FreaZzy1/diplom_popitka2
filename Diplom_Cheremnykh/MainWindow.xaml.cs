using System.Text;
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

namespace Diplom_Cheremnykh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public AppDbContext _context;
        public User _currentUser;
        public MainWindow()
        {
            InitializeComponent();

            _context = new AppDbContext();  // Создайте экземпляр контекста (или используйте DI)
            
            frame.Navigate(new Pages.LoginPage(this, _context));
        }
        public void OpenPages(Page page)
        {
            frame.Content = page;
        }
        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
    }
}