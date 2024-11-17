using InventoryManagement.ADOConsoleApp.Seeder;
using Microsoft.Data.SqlClient;

namespace InventoryManagement.Tests
{
    public class AdoNetAppTest 
    {
        private readonly DatabaseSeeder _databaseSeeder;

        public AdoNetAppTest()
        {
            _databaseSeeder = new DatabaseSeeder();
        }

        [Fact]
        public void GenerateCategories_ShouldReturnCorrectNumberOfCategories()
        {
            int count = 5;

            var categories = DatabaseSeeder.GenerateCategories(count);

            Assert.Equal(count, categories.Count);
            Assert.Contains("Electronics", categories);
            Assert.Contains("Furniture", categories);
        }

        [Fact]
        public void GenerateSuppliers_ShouldReturnCorrectNumberOfSuppliers()
        {
            int count = 5;

            var suppliers = DatabaseSeeder.GenerateSuppliers(count);

            Assert.Equal(count, suppliers.Count);
            Assert.Contains("Supplier A", suppliers);
            Assert.Contains("Supplier B", suppliers);
        }

        [Fact]
        public void GenerateProducts_ShouldCheckProductsCount()
        {
            var products = DatabaseSeeder.GenerateProducts();

            var productTitle1 = products.First(p => p.Title == "Apple iPhone 13").Title;
            var productTitle2 = products.First(p => p.Title == "Sony WH-1000XM4").Title;


            Assert.True(products.Count > 10);
            Assert.Equal("Apple iPhone 13", productTitle1);
            Assert.Equal("Sony WH-1000XM4", productTitle2);
        }

        [Fact]
        public void TestInsertAndRetrieveCategories_ShouldInsertDataIntoCategoriesTable()
        {
            _databaseSeeder.FillDatabaseWithTestData();

            using (SqlConnection connection = _databaseSeeder.GetConnection())
            {
                connection.Open();

                string checkCategoryQuery = "SELECT COUNT(*) FROM dbo.Categories";
                using (SqlCommand command = new SqlCommand(checkCategoryQuery, connection))
                {
                    var categoryCount = (int)command.ExecuteScalar();
                    Assert.True(categoryCount > 0, "Categories were not inserted into the database.");
                }
            }

            _databaseSeeder.CleanUpTestData();
        }

        [Fact]
        public void TestInsertAndRetrieveProducts_ShouldInsertDataIntoProductsTable()
        {
            _databaseSeeder.FillDatabaseWithTestData();

            using (SqlConnection connection = _databaseSeeder.GetConnection())
            {
                connection.Open();

                string checkProductQuery = "SELECT COUNT(*) FROM dbo.Products";
                using (SqlCommand command = new SqlCommand(checkProductQuery, connection))
                {
                    var productCount = (int)command.ExecuteScalar();
                    Assert.True(productCount > 0, "Products were not inserted into the database.");
                }
            }

            _databaseSeeder.CleanUpTestData();
        }

        [Fact]
        public void TestInsertAndRetrieveSuppliers_ShouldInsertDataIntoSuppliersTable()
        {
            _databaseSeeder.FillDatabaseWithTestData();

            using (SqlConnection connection = _databaseSeeder.GetConnection())
            {
                connection.Open();

                string checkSupplierQuery = "SELECT COUNT(*) FROM dbo.Suppliers";
                using (SqlCommand command = new SqlCommand(checkSupplierQuery, connection))
                {
                    var supplierCount = (int)command.ExecuteScalar();
                    Assert.True(supplierCount > 0, "Suppliers were not inserted into the database.");
                }
            }

            _databaseSeeder.CleanUpTestData();
        }

        [Fact]
        public void TestCleanUpTestData_ShouldRemoveAllTestDataFromTables()
        {
            _databaseSeeder.FillDatabaseWithTestData();
            _databaseSeeder.CleanUpTestData();

            using (SqlConnection connection = _databaseSeeder.GetConnection())
            {
                connection.Open();

                string checkCategoryQuery = "SELECT COUNT(*) FROM dbo.Categories";
                using (SqlCommand command = new SqlCommand(checkCategoryQuery, connection))
                {
                    var categoryCount = (int)command.ExecuteScalar();
                    Assert.Equal(0, categoryCount);
                }

                string checkProductQuery = "SELECT COUNT(*) FROM dbo.Products";
                using (SqlCommand command = new SqlCommand(checkProductQuery, connection))
                {
                    var productCount = (int)command.ExecuteScalar();
                    Assert.Equal(0, productCount);
                }

                string checkSupplierQuery = "SELECT COUNT(*) FROM dbo.Suppliers";
                using (SqlCommand command = new SqlCommand(checkSupplierQuery, connection))
                {
                    var supplierCount = (int)command.ExecuteScalar();
                    Assert.Equal(0, supplierCount);
                }
            }
        }
    }
}
