using InventoryManagement.DAL;
using System.Windows;

namespace InventoryManagement.UI
{
    public partial class MainWindow : Window
    {
        private readonly InventoryDbContext _context;

        // Constructor accepts InventoryDbContext
        public MainWindow(InventoryDbContext context)
        {
            InitializeComponent();
            _context = context; // Assign context to the private field
        }
    }
}
