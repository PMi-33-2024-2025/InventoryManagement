using InventoryManagement.DAL;
using InventoryManagementApp.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //DatabaseSeeder.FillDatabaseWithTestData();
            //Console.WriteLine("Data was inserted successfully!");

            //DatabaseSeeder.DisplayData("Categories", "SELECT Id, Name FROM dbo.Categories");
            //DatabaseSeeder.DisplayData("Suppliers", "SELECT Id, Name FROM dbo.Suppliers");
            //DatabaseSeeder.DisplayData("Products", "SELECT Id, Title, Amount, Price, Description FROM dbo.Products");

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            await SeedData(serviceProvider);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer("YourConnectionString"));

            services.AddIdentity<InventoryUser, IdentityRole>()
                .AddEntityFrameworkStores<InventoryDbContext>()
                .AddDefaultTokenProviders();
        }

        private static async Task SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<InventoryUser>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var adminUser = new InventoryUser { UserName = "admin@example.com" };
                if (await userManager.FindByNameAsync(adminUser.UserName) == null)
                {
                    await userManager.CreateAsync(adminUser, "AdminPassword123!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
