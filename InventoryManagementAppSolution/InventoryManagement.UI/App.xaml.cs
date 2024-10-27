using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.DAL;
using InventoryManagement.BLL;
using Microsoft.AspNetCore.Identity;
using System.Windows;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace InventoryManagement.UI
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys"))
                .SetApplicationName("InventoryManagement");

            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryManagement;Integrated Security=True;"));

            services.AddIdentity<InventoryUser, IdentityRole>()
                .AddEntityFrameworkStores<InventoryDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<AuthService>();
            services.AddScoped<InventoryService>();
			services.AddLogging();

            services.AddAuthentication();

			ServiceProvider = services.BuildServiceProvider();
            AuthService.ServiceProvider = ServiceProvider;
        }
    }
}
