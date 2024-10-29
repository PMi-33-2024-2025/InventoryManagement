using InventoryManagement.BLL;
using InventoryManagement.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.UI
{
    public partial class InventoryWindow : Window, INotifyPropertyChanged
    {
        private readonly InventoryService _inventoryService;
        private readonly AuthService _authService;

        private List<Product>? _products;
        private List<string>? _categories = ["Усі"];
        private decimal _minPrice;
        private decimal _maxPrice;
        private decimal _selectedMinPrice;
        private decimal _selectedMaxPrice;
        private bool _isInStock;
        private Product _currentEditingProduct;
        public bool IsEditing { get; set; } = false;

        public List<Product>? Products
        {
            get => _products;
            private set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public List<string>? Categories
        {
            get => _categories;
            private set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public decimal MinPrice
        {
            get => _minPrice;
            private set
            {
                _minPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal MaxPrice
        {
            get => _maxPrice;
            private set
            {
                _maxPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedMinPrice
        {
            get => _selectedMinPrice;
            set
            {
                _selectedMinPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal SelectedMaxPrice
        {
            get => _selectedMaxPrice;
            set
            {
                _selectedMaxPrice = value;
                OnPropertyChanged();
            }
        }

        public bool IsInStock
        {
            get => _isInStock;
            set
            {
                _isInStock = value;
                OnPropertyChanged();
            }
        }

        public InventoryWindow()
        {
            InitializeComponent();
            _inventoryService = App.ServiceProvider.GetRequiredService<InventoryService>();
            _authService = App.ServiceProvider.GetRequiredService<AuthService>();
            DataContext = this;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ReloadPage();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMinPrice > SelectedMaxPrice)
            {
                MessageBox.Show("Мінімальна ціна не повинна перевищувати максимальну ціну.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await LoadFilteredCollectionsAsync();
        }

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            textBoxNameContains.Text = string.Empty;
            comboBoxCategory.SelectedIndex = -1;
            SelectedMinPrice = MinPrice;
            SelectedMaxPrice = MaxPrice;
            IsInStock = false;
            await LoadCollectionsAsync();
        }

        private async Task LoadCollectionsAsync()
        {
            Categories = (await _inventoryService.GetCategoriesAsync()).Select(c => c.Name).ToList();
            Categories.Insert(0, "Усі");
            comboBoxCategory.SelectedIndex = 0;
            Products = await _inventoryService.GetProductsAsync();
        }

        private async Task LoadFilteredCollectionsAsync()
        {
            int index = comboBoxCategory.SelectedIndex;
            string category = (comboBoxCategory.SelectedItem as string)!;

            Products = await _inventoryService.GetFilteredProductsAsync(
                Filters.NameContains(textBoxNameContains.Text.Trim()),
                Filters.HasCategory(category.Equals("Усі", StringComparison.Ordinal) ? string.Empty : category),
                Filters.HasMinPrice(SelectedMinPrice),
                Filters.HasMaxPrice(SelectedMaxPrice),
                IsInStock ? Filters.IsInStock() : _ => true
            );

            comboBoxCategory.SelectedIndex = index;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new() { Owner = this };
            loginWindow.ShowDialog();
            await ReloadPage();
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow addItemWindow = new(
                async () => await ReloadPage()
            )
            { Owner = this };
            addItemWindow.ShowDialog();
        }

        private void AdminPanelButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new() { Owner = this };
            adminPanel.ShowDialog();
        }

        private async Task ReloadPage()
        {
            await LoadCollectionsAsync();
            SelectedMinPrice = MinPrice = Products?.Min(p => p.Price) ?? 0;
            SelectedMaxPrice = MaxPrice = Products?.Max(p => p.Price) ?? 0;

            string role = await _authService.GetCurrentUserRoleAsync();

            if (role == "User") ConfigureForUser();
            else if (role == "Admin") ConfigureForAdmin();
            else ConfigureForVisitor();
        }

        private void ConfigureForVisitor()
        {
            usernameText.Text = "";
            addItemButton.Visibility = Visibility.Collapsed;
            loginButton.Visibility = Visibility.Visible;
            usernameText.Visibility = Visibility.Collapsed;
            logoutButton.Visibility = Visibility.Collapsed;
            editColumn.Visibility = Visibility.Collapsed;
            deleteColumn.Visibility = Visibility.Collapsed;
            adminPanelButton.Visibility = Visibility.Collapsed;
            productsDataGrid.IsReadOnly = true;
        }

        private void ConfigureForUser()
        {
            usernameText.Text = _authService.GetCurrentUserName();
            addItemButton.Visibility = Visibility.Visible;
            loginButton.Visibility = Visibility.Collapsed;
            usernameText.Visibility = Visibility.Visible;
            logoutButton.Visibility = Visibility.Visible;
            editColumn.Visibility = Visibility.Visible;
            deleteColumn.Visibility = Visibility.Visible;
            adminPanelButton.Visibility = Visibility.Collapsed;
            productsDataGrid.IsReadOnly = false;
        }

        private void ConfigureForAdmin()
        {
            ConfigureForUser();
            adminPanelButton.Visibility = Visibility.Visible;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _authService.LogoutUser();
            await ReloadPage();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Product product)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Ви впевнені, що хочете видалити {product.Title}?",
                    "Підтвердження видалення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _inventoryService.DeleteProductAsync(product.Id);
                    await ReloadPage();
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Product product)
            {
                await _inventoryService.UpdateProductAsync(product);

                await ReloadPage();
            }
        }

    }
}
