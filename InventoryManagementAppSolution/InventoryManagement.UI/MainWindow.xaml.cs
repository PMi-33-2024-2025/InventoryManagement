using InventoryManagement.DAL;
using System.Windows;

namespace InventoryManagement.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InventoryDbContext _context = new InventoryDbContext();
        
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}