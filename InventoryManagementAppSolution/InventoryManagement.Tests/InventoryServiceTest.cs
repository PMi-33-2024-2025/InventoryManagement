using EntityFrameworkCoreMock;
using FluentAssertions;
using InventoryManagement.BLL;
using InventoryManagement.DAL;
using InventoryManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace InventoryManagement.Tests
{
    public class InventoryServiceTest
    {
        private readonly InventoryService _inventoryService;
        private readonly DbContextMock<InventoryDbContext> _dbContextMock;
        private readonly Mock<ILogger<InventoryService>> _loggerMock;

        public InventoryServiceTest()
        {
            _dbContextMock = new DbContextMock<InventoryDbContext>(new DbContextOptionsBuilder<InventoryDbContext>().Options);
            _loggerMock = new Mock<ILogger<InventoryService>>();

			var initialProducts = new List<Product>();
            _dbContextMock.CreateDbSetMock(x => x.Products, initialProducts);
            var initialCategories = new List<Category>();
            _dbContextMock.CreateDbSetMock(x => x.Categories, initialCategories);
            var initialSuppliers = new List<Supplier>();
            _dbContextMock.CreateDbSetMock(x => x.Suppliers, initialSuppliers);

            _inventoryService = new InventoryService(_dbContextMock.Object, _loggerMock.Object);
        }

        #region GetProductsAsync
        [Fact]
        public async Task GetProductsAsync_ShouldReturnAllProducts()
        {
            var expectedProducts = _dbContextMock.Object.Products;
            expectedProducts.Add(new Product { Id = 1, Title = "Product A", Amount = 10, Price = 100 });
            expectedProducts.Add(new Product { Id = 2, Title = "Product B", Amount = 20, Price = 200 });
            expectedProducts.Add(new Product { Id = 3, Title = "Product C", Amount = 30, Price = 300 });

            await _dbContextMock.Object.SaveChangesAsync();

            var actualProducts = await _inventoryService.GetProductsAsync();

            actualProducts.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            var actualProducts = await _inventoryService.GetProductsAsync();

            actualProducts.Should().BeEmpty();
        }
        #endregion

        #region GetCategoriesAsync
        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnAllCategories()
        {
            var expectedCategories = _dbContextMock.Object.Categories;
            expectedCategories.Add(new Category { Id = 1, Name = "Category A" });
            expectedCategories.Add(new Category { Id = 2, Name = "Category B" });
            expectedCategories.Add(new Category { Id = 3, Name = "Category C" });

            await _dbContextMock.Object.SaveChangesAsync();

            var actualCategories = await _inventoryService.GetCategoriesAsync();

            actualCategories.Should().BeEquivalentTo(expectedCategories);
        }

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var actualCategories = await _inventoryService.GetCategoriesAsync();

            actualCategories.Should().BeEmpty();
        }
        #endregion

        #region GetFilteredProductsAsync
        [Fact]
        public async Task GetFilteredProductsAsync_ShouldReturnFilteredProducts()
        {
            var expectedProducts = _dbContextMock.Object.Products;
            expectedProducts.Add(new Product { Id = 1, Title = "Product A", Amount = 10, Price = 100 });
            expectedProducts.Add(new Product { Id = 2, Title = "Product B", Amount = 20, Price = 200 });
            expectedProducts.Add(new Product { Id = 3, Title = "Product C", Amount = 30, Price = 300 });

            await _dbContextMock.Object.SaveChangesAsync();

            var actualProducts = await _inventoryService.GetFilteredProductsAsync(p => p.Amount > 10);

            actualProducts.Should().BeEquivalentTo(expectedProducts.Skip(1));
        }

        [Fact]
        public async Task GetFilteredProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            var actualProducts = await _inventoryService.GetFilteredProductsAsync(p => p.Amount > 10);

            actualProducts.Should().BeEmpty();
        }
        #endregion

        #region GetProductAsync
        [Fact]
        public async Task GetProductAsync_ShouldReturnProduct_WhenProductExists()
        {
            var expectedProduct = new Product { Id = 1, Title = "Product A", Amount = 10, Price = 100 };
            _dbContextMock.Object.Products.Add(expectedProduct);

            await _dbContextMock.Object.SaveChangesAsync();

            var actualProduct = await _inventoryService.GetProductAsync(1);

            actualProduct.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            var actualProduct = await _inventoryService.GetProductAsync(1);

            actualProduct.Should().BeNull();
        }
        #endregion

        #region AddProductAsync
        [Fact]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            var newProduct = new Product { Id = 1, Title = "Product A", Amount = 10, Price = 100 };

            await _inventoryService.AddProductAsync(newProduct);

            _dbContextMock.Object.Products.Should().Contain(newProduct);
        }
        #endregion

        #region UpdateProductAsync
        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            var productToUpdate = new Product { Id = 1, Title = "Product A", Amount = 10, Price = 100 };
            _dbContextMock.Object.Products.Add(productToUpdate);

            await _dbContextMock.Object.SaveChangesAsync();

            productToUpdate.Title = "Product B";
            await _inventoryService.UpdateProductAsync(productToUpdate);

            _dbContextMock.Object.Products.Should().Contain(productToUpdate);
        }
        #endregion

        #region DeleteProductAsync
        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            var productToDelete = new Product { Id = 1, Title = "Product A", Amount = 10, Price = 100 };
            _dbContextMock.Object.Products.Add(productToDelete);

            await _dbContextMock.Object.SaveChangesAsync();

            await _inventoryService.DeleteProductAsync(1);

            _dbContextMock.Object.Products.Should().NotContain(productToDelete);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            var act = () => _inventoryService.DeleteProductAsync(1);

            await act.Should().ThrowAsync<ArgumentException>();
        }
        #endregion

        #region CreateCategoryIfNotExists
        [Fact]
        public async Task CreateCategoryIfNotExists_ShouldReturnExistingCategory_WhenCategoryExists()
        {
            var existingCategory = new Category { Id = 1, Name = "Category A" };
            _dbContextMock.Object.Categories.Add(existingCategory);

            await _dbContextMock.Object.SaveChangesAsync();

            var actualCategory = await _inventoryService.CreateCategoryIfNotExists("Category A");

            actualCategory.Should().Be(existingCategory);
        }

        [Fact]
        public async Task CreateCategoryIfNotExists_ShouldCreateNewCategory_WhenCategoryDoesNotExist()
        {
            var newCategory = new Category { Id = 1, Name = "Category A" };

            var actualCategory = await _inventoryService.CreateCategoryIfNotExists("Category A");

            actualCategory.Should().BeEquivalentTo(newCategory);
        }
        #endregion

        #region CreateSupplierIfNotExists
        [Fact]
        public async Task CreateSupplierIfNotExists_ShouldReturnExistingSupplier_WhenSupplierExists()
        {
            var existingSupplier = new Supplier { Id = 1, Name = "Supplier A" };
            _dbContextMock.Object.Suppliers.Add(existingSupplier);

            await _dbContextMock.Object.SaveChangesAsync();

            var actualSupplier = await _inventoryService.CreateSupplierIfNotExists("Supplier A");

            actualSupplier.Should().Be(existingSupplier);
        }

        [Fact]
        public async Task CreateSupplierIfNotExists_ShouldCreateNewSupplier_WhenSupplierDoesNotExist()
        {
            var newSupplier = new Supplier { Id = 1, Name = "Supplier A" };

            var actualSupplier = await _inventoryService.CreateSupplierIfNotExists("Supplier A");

            actualSupplier.Should().BeEquivalentTo(newSupplier);
        }
        #endregion
    }
}