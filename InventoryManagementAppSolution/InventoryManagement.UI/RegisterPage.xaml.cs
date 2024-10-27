using System.Windows.Controls;

namespace InventoryManagement.UI
{
	public partial class RegisterPage : Page
	{
		public Action NavigateToLogin { get; set; }
		public Action CloseWindow { get; set; }
		public RegisterPage(Action navigateToLogin, Action closeWindow)
		{
			InitializeComponent();
			NavigateToLogin = navigateToLogin;
			CloseWindow = closeWindow;
		}

		private void loginLink_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NavigateToLogin();
		}
	}
}
