using InventoryManagement.BLL;
using InventoryManagement.BLL.Helpers;
using InventoryManagement.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;

namespace InventoryManagement.UI
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
	{
        private readonly InventoryService _inventoryService;
        public Action ReloadPage { get; set; }
		public AddItemWindow(Action action)
		{
			InitializeComponent();
            _inventoryService = App.ServiceProvider.GetRequiredService<InventoryService>();
            ReloadPage = action;
        }

        private async void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string category = CategoryTextBox.Text;
            string description = DescriptionTextBox.Text;
            string quantityText = QuantityTextBox.Text;
            string priceText = PriceTextBox.Text;
            string supplier = SupplierTextBox.Text;

            StringBuilder errorMessages = new StringBuilder();

            if (!Validator.IsStringValid(name))
            {
                errorMessages.AppendLine("Назва не може бути пустою.");
            }

            if (!Validator.IsStringValid(category))
            {
                errorMessages.AppendLine("Категорія не може бути пустою.");
            }

            if (!Validator.IsStringValid(description))
            {
                errorMessages.AppendLine("Опис не може бути пустим.");
            }

            if (!int.TryParse(quantityText, out int quantity) || !Validator.IsIntValid(quantity))
            {
                errorMessages.AppendLine("Кількість повинна бути цілим числом і не може бути від'ємною.");
            }

            if (!decimal.TryParse(priceText, out decimal price) || !Validator.IsDecimalValid(price))
            {
                errorMessages.AppendLine("Ціна повинна бути числом більше 0.");
            }

            if (!Validator.IsStringValid(supplier))
            {
                errorMessages.AppendLine("Постачальник не може бути пустим.");
            }

            if (errorMessages.Length > 0)
            {
                MessageBox.Show(errorMessages.ToString(), "Помилки введення даних", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var categoryNew = await _inventoryService.CreateCategoryIfNotExists(category);
            var supplierNew = await _inventoryService.CreateSupplierIfNotExists(supplier);

            Product product = new Product
            {
                Title = name,
                Category = categoryNew,
                Description = description,
                Amount = quantity,
                Price = price,
                Supplier = supplierNew,
                LastUpdated = DateTime.Now
            };

            await _inventoryService.AddProductAsync(product);
            Close();
            ReloadPage();
        }
    }
}
