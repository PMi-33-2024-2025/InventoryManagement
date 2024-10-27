using System.Windows.Controls;

namespace InventoryManagement.UI
{
	public partial class RegisterPage : Page
	{
		public Action NavigateToLogin { get; set; }
		public RegisterPage(Action navigateToLogin)
		{
			InitializeComponent();
			NavigateToLogin = navigateToLogin;
		}

		private void loginLink_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NavigateToLogin();
		}
	}
}
