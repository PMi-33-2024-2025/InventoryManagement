using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;
using System.Windows;
using System;

namespace InventoryManagement.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryManagement;Integrated Security=True;"));

            services.AddIdentity<InventoryUser, IdentityRole>()
                .AddEntityFrameworkStores<InventoryDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<AuthService>();

            var serviceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow(serviceProvider.GetRequiredService<InventoryDbContext>());
            mainWindow.Show();
        }
    }
}
