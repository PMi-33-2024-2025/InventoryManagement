using InventoryManagement.BLL;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.UI
{
	public partial class RegisterPage : Page
	{
		private readonly AuthService _authService;
		public Action NavigateToLogin { get; set; }
		public Action CloseWindow { get; set; }
		public RegisterPage(Action navigateToLogin, Action closeWindow)
		{
			InitializeComponent();
			NavigateToLogin = navigateToLogin;
			CloseWindow = closeWindow;
			_authService = App.ServiceProvider.GetRequiredService<AuthService>();
		}

		private void loginLink_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NavigateToLogin();
		}

		private async void RegisterButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			await RegisterUser();
		}

		private async Task RegisterUser()
		{
			var username = usernameTextBox.Text;
			var password = passwordBox.Password;
			var confirmPassword = confirmPasswordBox.Password;

			if (!password.Equals(confirmPassword, StringComparison.Ordinal))
			{
				MessageBox.Show("Паролі у полі 'Пароль' та 'Підтвердіть пароль' повинні співпадати",
					"Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (await _authService.UserExists(username))
			{
				MessageBox.Show("Користувач із заданим логіном уже існує.",
					"Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				await _authService.RegisterUserAsync(username, password);
				await _authService.LoginUserAsync(username, password);
			}
			catch (Exception)
			{
				MessageBox.Show("Сталася неочікувана помилка.",
					"Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				CloseWindow();
			}
		}
	}
}
