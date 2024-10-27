using InventoryManagement.BLL;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace InventoryManagement.UI
{
	public partial class LoginWindow : Window
	{
		private readonly AuthService _authService;
		public LoginPage LoginPage { get; set; }
		public RegisterPage RegisterPage { get; set; }

		public LoginWindow()
		{
			InitializeComponent();
			_authService = App.ServiceProvider.GetRequiredService<AuthService>();
			LoginPage = new(NavigateToRegister);
			RegisterPage = new(NavigateToLogin);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			NavigateToLogin();
		}

		public void NavigateToRegister()
		{
			MainFrame.Navigate(RegisterPage);
		}

		public void NavigateToLogin()
		{
			MainFrame.Navigate(LoginPage);
		}
	}
}
