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

namespace Diplom_Cheremnykh.Pages
{
    /// <summary>
    /// Логика взаимодействия для FraudCasesPage.xaml
    /// </summary>
    public partial class FraudCasesPage : Page
    {
        public MainWindow _mainWindow;
        private AppDbContext _context;
        public FraudCasesPage(MainWindow mainWindow, AppDbContext context)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _context = context;
        }
    }
}
