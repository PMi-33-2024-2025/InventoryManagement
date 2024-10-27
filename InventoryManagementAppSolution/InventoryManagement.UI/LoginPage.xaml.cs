using System.Windows.Controls;

namespace InventoryManagement.UI
{
	public partial class LoginPage : Page
	{
		public Action NavigateToRegister { get; set; }
		public LoginPage(Action navigateToRegister)
		{
			InitializeComponent();
			NavigateToRegister = navigateToRegister;
		}

		private void registerLink_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NavigateToRegister();
		}
	}
}
