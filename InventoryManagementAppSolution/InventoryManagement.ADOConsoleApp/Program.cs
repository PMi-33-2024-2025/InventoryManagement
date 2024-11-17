using InventoryManagement.ADOConsoleApp.Seeder;
using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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

            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/logfile.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            try
            {
                Log.Information("Application Starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog();

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

				if(!await roleManager.RoleExistsAsync("User"))
				{
					await roleManager.CreateAsync(new IdentityRole("User"));
				}

				var adminUser = new InventoryUser { UserName = "admin@example.com" };
				if (await userManager.FindByNameAsync(adminUser.UserName) == null)
				{
					await userManager.CreateAsync(adminUser, "AdminPassword123!");
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}

				var user = new InventoryUser { UserName = "user@example.com" };
				if (await userManager.FindByNameAsync(user.UserName) == null)
				{
					await userManager.CreateAsync(user, "UserPassword123!");
					await userManager.AddToRoleAsync(user, "User");
				}
			}

			DatabaseSeeder.FillDatabaseWithTestData();
		}
	}
}
