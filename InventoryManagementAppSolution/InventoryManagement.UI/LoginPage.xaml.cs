using InventoryManagement.BLL;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.UI
{
	public partial class LoginPage : Page
	{
		private readonly AuthService _authService;
		public Action NavigateToRegister { get; set; }
		public Action CloseWindow { get; set; }
		public LoginPage(Action navigateToRegister, Action closeWindow)
		{
			InitializeComponent();
			NavigateToRegister = navigateToRegister;
			CloseWindow = closeWindow;
			_authService = App.ServiceProvider.GetRequiredService<AuthService>();
		}

		private void registerLink_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NavigateToRegister();
		}

		private async void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			await LoginUser();
		}

		private async Task LoginUser()
		{
			var username = usernameTextBox.Text;
			var password = passwordBox.Password;
			var result = await _authService.LoginUserAsync(username, password);

			if (result.Succeeded)
			{
				CloseWindow();
			}
			else
			{
				MessageBox.Show("Неправильне ім'я користувача або пароль. Попробуйте ще раз.",
					"Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
