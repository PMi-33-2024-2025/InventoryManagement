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
			=> await _db.Products
			.Include(p => p.Supplier)
			.Include(p => p.Category)
			.AsNoTracking()
			.ToListAsync();

		public async Task<List<Category>> GetCategoriesAsync()
			=> await _db.Categories
			.AsNoTracking()
			.ToListAsync();

		public async Task<List<Product>> GetFilteredProductsAsync(params Func<Product, bool>[] predicates)
			=> (await GetProductsAsync())
			.Where(product => predicates.All(predicate => predicate(product)))
			.ToList();

		public async Task<Product?> GetProductAsync(int id)
			=> await _db.Products
			.Include(p => p.Supplier)
			.Include(p => p.Category)
			.AsNoTracking()
			.FirstAsync(p => p.Id == id);

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

        public async Task<Category> CreateCategoryIfNotExists(string categoryName)
        {
            var existingCategory = _db.Categories.AsEnumerable().FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.Ordinal));
			if (existingCategory is not null)
            {
                return existingCategory;
            }

            var newCategory = new Category { Name = categoryName };
            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();

            return newCategory;
        }

        public async Task<Supplier> CreateSupplierIfNotExists(string supplierName)
        {
            var existingSupplier = _db.Suppliers.AsEnumerable().FirstOrDefault(s => s.Name.Equals(supplierName, StringComparison.Ordinal));
            if (existingSupplier is not null)
			{
				return existingSupplier;
			}

            var newSupplier = new Supplier { Name = supplierName };
            await _db.Suppliers.AddAsync(newSupplier);
            await _db.SaveChangesAsync();

            return newSupplier;
        }
	}
}
