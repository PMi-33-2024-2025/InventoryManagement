using InventoryManagement.DAL;
using InventoryManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.BLL
{
	public class InventoryService
	{
		private readonly InventoryDbContext _db;

		public InventoryService(InventoryDbContext db)
		{
			_db = db;
		}

		public async Task<List<Product>> GetProductsAsync()
			=> await _db.Products.AsNoTracking().ToListAsync();

		public async Task<List<Product>> GetOrderedProductsAsync(IComparer<Product> comparer)
			=> (await GetProductsAsync()).Order(comparer).ToList();

		public async Task<List<Product>> GetFilteredProductsAsync(Func<Product, bool> predicate)
			=> (await GetProductsAsync()).Where(predicate).ToList();

		public async Task<List<Product>> GetOrderedAndFilteredProductsAsync(IComparer<Product> comparer, Func<Product, bool> predicate)
			=> (await GetProductsAsync()).Order(comparer).Where(predicate).ToList();

		public async Task<Product?> GetProductAsync(int id)
			=> await _db.Products.AsNoTracking().FirstAsync(p => p.Id == id);

		public async Task AddProductAsync(Product product)
		{
			await _db.Products.AddAsync(product);
			await _db.SaveChangesAsync();
		}

		public async Task UpdateProductAsync(Product product)
		{
			_db.Products.Update(product);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(int id)
		{
			Product productToDelete = await GetProductAsync(id) ?? throw new ArgumentException("Product not found");
			_db.Products.Remove(productToDelete);
			await _db.SaveChangesAsync();
		}
	}
}
