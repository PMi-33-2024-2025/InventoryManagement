using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.DAL;
using InventoryManagement.BLL;
using Microsoft.AspNetCore.Identity;
using System.Windows;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Serilog;

namespace InventoryManagement.UI
{
	public partial class App : Application
	{
		public static ServiceProvider ServiceProvider { get; private set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Log.Logger = new LoggerConfiguration()
				.WriteTo.File("logs/logfile.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			var services = new ServiceCollection();
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddSerilog(dispose: true);
			});

			services.AddDataProtection()
				.PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys"))
				.SetApplicationName("InventoryManagement");

			services.AddDbContext<InventoryDbContext>(options =>
				options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryManagement;Integrated Security=True;"));

			services.AddIdentity<InventoryUser, IdentityRole>()
				.AddEntityFrameworkStores<InventoryDbContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<AuthService>();
			services.AddScoped<InventoryService>();

			ServiceProvider = services.BuildServiceProvider();

			Log.Information("Application started");
		}

		protected override void OnExit(ExitEventArgs e)
		{
			Log.Information("Application shutting down");
			Log.CloseAndFlush();
			base.OnExit(e);
		}
	}
}
