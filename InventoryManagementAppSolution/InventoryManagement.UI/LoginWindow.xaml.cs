using System.Windows;

namespace InventoryManagement.UI
{
	public partial class LoginWindow : Window
	{
		public LoginPage LoginPage { get; set; }
		public RegisterPage RegisterPage { get; set; }

		public LoginWindow()
		{
			InitializeComponent();
			LoginPage = new(NavigateToRegister, Close);
			RegisterPage = new(NavigateToLogin, Close);
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
