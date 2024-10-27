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

        public async Task<Category> CreateCategoryIfNonExist(string categoryName)
        {
            if (await _db.Categories.AnyAsync(c => c.Name == categoryName))
            {
                return await _db.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            }

            var newCategory = new Category { Name = categoryName };
            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();

            return await _db.Categories.FirstOrDefaultAsync(c => c.Name == categoryName) ?? newCategory;
        }

        public async Task<Supplier> CreateSupplierIfNonExist(string supplierName)
        {
            var existingSupplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.Name == supplierName);
            if (existingSupplier != null)
            {
                return existingSupplier;
            }

            var newSupplier = new Supplier { Name = supplierName };
            await _db.Suppliers.AddAsync(newSupplier);
            await _db.SaveChangesAsync();
            return await _db.Suppliers.FirstOrDefaultAsync(s => s.Name == supplierName) ?? newSupplier;
        }
	}
}
