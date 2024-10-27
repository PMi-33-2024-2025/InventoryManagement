using InventoryManagement.DAL.Entities;

namespace InventoryManagement.BLL
{
	public static class Filters
	{
		public static Func<Product, bool> NameContains(string name) =>
			(Product p) => name.Equals(string.Empty)
			|| p.Title.Contains(name, StringComparison.OrdinalIgnoreCase);

		public static Func<Product, bool> HasCategory(string category) =>
			(Product p) => category.Equals(string.Empty)
			|| p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase);

		public static Func<Product, bool> HasMinPrice(decimal minPrice) =>
			(Product p) => p.Price >= minPrice;

		public static Func<Product, bool> HasMaxPrice(decimal maxPrice) =>
			(Product p) => p.Price <= maxPrice;

		public static Func<Product, bool> IsInStock() =>
			(Product p) => p.Amount > 0;
	}
}
