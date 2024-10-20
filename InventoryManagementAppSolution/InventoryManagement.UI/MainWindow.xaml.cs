using InventoryManagement.DAL;
using System.Windows;

namespace InventoryManagement.UI
{
    public partial class MainWindow : Window
    {
        private readonly InventoryDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(InventoryDbContext context) : this()
        {
            _context = context;
        }
    }
}
