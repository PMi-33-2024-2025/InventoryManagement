using InventoryManagement.ADOConsoleApp.Seeder;
using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.ADOConsoleApp
{
	public class Program
	{
		static async Task Main(string[] args)
		{
			//var serviceCollection = new ServiceCollection();
			//ConfigureServices(serviceCollection);
			//var serviceProvider = serviceCollection.BuildServiceProvider();

			//await SeedData(serviceProvider);
			//Console.WriteLine("Data was inserted successfully!");

			DatabaseSeeder.DisplayData("Categories", "SELECT Id, Name FROM dbo.Categories");
			DatabaseSeeder.DisplayData("Suppliers", "SELECT Id, Name FROM dbo.Suppliers");
			DatabaseSeeder.DisplayData("Products", "SELECT Id, Title, Amount, Price, Description FROM dbo.Products");
			DatabaseSeeder.DisplayData("AspNetRoles", "SELECT * FROM dbo.AspNetRoles");
			DatabaseSeeder.DisplayData("AspNetUsers", "SELECT * FROM dbo.AspNetUsers");
			DatabaseSeeder.DisplayData("AspNetUserRoles", "SELECT * FROM dbo.AspNetUserRoles");
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<InventoryDbContext>(options =>
				options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=InventoryManagement;Trusted_Connection=True;"));

			services.AddIdentity<InventoryUser, IdentityRole>()
				.AddEntityFrameworkStores<InventoryDbContext>()
				.AddDefaultTokenProviders();

			services.AddLogging();
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

			DatabaseSeeder.FillDatabaseWithTestData();
		}
	}
}
