using InventoryManagement.DAL;
using InventoryManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.BLL
{
    public class InventoryService
    {
        private readonly InventoryDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(InventoryDbContext db, ILogger<InventoryService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            _logger.LogInformation("Getting all products from the database.");
            var products = await _db.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
            _logger.LogInformation("Successfully fetched {ProductCount} products.", products.Count);
            return products;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            _logger.LogInformation("Getting all categories from the database.");
            var categories = await _db.Categories
                .AsNoTracking()
                .ToListAsync();
            _logger.LogInformation("Successfully fetched {CategoryCount} categories.", categories.Count);
            return categories;
        }

        public async Task<List<Product>> GetFilteredProductsAsync(params Func<Product, bool>[] predicates)
        {
            _logger.LogInformation("Getting filtered products based on provided criteria.");
            var products = await GetProductsAsync();
            var filteredProducts = products.Where(product => predicates.All(predicate => predicate(product))).ToList();
            _logger.LogInformation("Successfully fetched {FilteredProductCount} products.", filteredProducts.Count);
            return filteredProducts;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            _logger.LogInformation("Getting product with ID {ProductId}.", id);
            var product = await _db.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", id);
            }
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            _logger.LogInformation("Adding new product with title '{ProductTitle}' and ID {ProductId}.", product.Title, product.Id);
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Successfully added product with ID {ProductId}.", product.Id);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _logger.LogInformation("Updating product with ID {ProductId}.", product.Id);
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Product with ID {ProductId} updated successfully.", product.Id);
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                Product productToDelete = await GetProductAsync(id) ?? throw new ArgumentException("Product not found");
                _logger.LogInformation("Deleting product with ID {ProductId}.", id);
                _db.Products.Remove(productToDelete);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Product with ID {ProductId} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID {ProductId}.", id);
                throw;
            }
        }

        public async Task<Category> CreateCategoryIfNotExists(string categoryName)
        {
            _logger.LogInformation("Checking if category '{CategoryName}' exists.", categoryName);
            var existingCategory = _db.Categories.AsEnumerable().FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.Ordinal));
            if (existingCategory != null)
            {
                _logger.LogInformation("Category '{CategoryName}' already exists with ID {CategoryId}.", categoryName, existingCategory.Id);
                return existingCategory;
            }

            _logger.LogInformation("Category '{CategoryName}' does not exist. Creating new category.", categoryName);
            var newCategory = new Category { Name = categoryName };
            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();
            _logger.LogInformation("New category '{CategoryName}' created with ID {CategoryId}.", categoryName, newCategory.Id);
            return newCategory;
        }

        public async Task<Supplier> CreateSupplierIfNotExists(string supplierName)
        {
            _logger.LogInformation("Checking if supplier '{SupplierName}' exists.", supplierName);
            var existingSupplier = _db.Suppliers.AsEnumerable().FirstOrDefault(s => s.Name.Equals(supplierName, StringComparison.Ordinal));
            if (existingSupplier != null)
            {
                _logger.LogInformation("Supplier '{SupplierName}' already exists with ID {SupplierId}.", supplierName, existingSupplier.Id);
                return existingSupplier;
            }

            _logger.LogInformation("Supplier '{SupplierName}' does not exist. Creating new supplier.", supplierName);
            var newSupplier = new Supplier { Name = supplierName };
            await _db.Suppliers.AddAsync(newSupplier);
            await _db.SaveChangesAsync();
            _logger.LogInformation("New supplier '{SupplierName}' created with ID {SupplierId}.", supplierName, newSupplier.Id);
            return newSupplier;
        }
    }
}
