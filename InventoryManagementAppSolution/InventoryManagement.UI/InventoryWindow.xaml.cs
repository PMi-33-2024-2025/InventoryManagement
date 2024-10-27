using InventoryManagement.BLL;
using InventoryManagement.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InventoryManagement.UI
{
	public partial class InventoryWindow : Window, INotifyPropertyChanged
	{
		private readonly InventoryService _inventoryService;

		private List<Product>? _products;
		private List<string>? _categories = ["Усі"];
		private decimal _minPrice;
		private decimal _maxPrice;
		private decimal _selectedMinPrice;
		private decimal _selectedMaxPrice;
		private bool _isInStock;

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
			DataContext = this;
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			await LoadCollectionsAsync();
			SelectedMinPrice = MinPrice = Products?.Min(p => p.Price) ?? 0;
			SelectedMaxPrice = MaxPrice = Products?.Max(p => p.Price) ?? 0;
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
				MessageBox.Show("Мінімальна ціна не повинна перевищувати максимальну.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoginWindow loginWindow = new() { Owner = this };
			loginWindow.ShowDialog();
		}

		private void AddItemButton_Click(object sender, RoutedEventArgs e)
		{
			AddItemWindow addItemWindow = new(ReloadPage)
			{ Owner = this };
			addItemWindow.ShowDialog();
		}

		private async void ReloadPage()
		{
			await LoadCollectionsAsync();
			SelectedMinPrice = MinPrice = Products?.Min(p => p.Price) ?? 0;
			SelectedMaxPrice = MaxPrice = Products?.Max(p => p.Price) ?? 0;
		}
	}
}
