using InventoryManagementApp.Seeder;

namespace InventoryManagementApp
{
    public class Program
    {
        public static void Main()
        {
            DatabaseSeeder.FillDatabaseWithTestData();
            Console.WriteLine("Data was inserted successfully!");

            DatabaseSeeder.DisplayData("Categories", "SELECT Id, Name FROM dbo.Categories");
            DatabaseSeeder.DisplayData("Suppliers", "SELECT Id, Name FROM dbo.Suppliers");
            DatabaseSeeder.DisplayData("Products", "SELECT Id, Title, Amount, Price, Description FROM dbo.Products");
        }
    }
}