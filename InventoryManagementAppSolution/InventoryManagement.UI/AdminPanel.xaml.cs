using InventoryManagement.BLL;
using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventoryManagement.UI
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private readonly AuthService _authService;

        public AdminPanel()
        {
            InitializeComponent();
            _authService = App.ServiceProvider.GetRequiredService<AuthService>();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _authService.GetUserRolesAsync(user);
                var roleDisplay = string.Join(", ", roles);

                userViewModels.Add(new UserViewModel
                {
                    UserName = user.UserName,
                    RoleDisplay = roleDisplay
                });
            }

            UsersDataGrid.ItemsSource = userViewModels;
        }

        private async void ChangeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is UserViewModel selectedUserViewModel)
            {
                var selectedRole = (RolesComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (string.IsNullOrEmpty(selectedRole))
                {
                    MessageBox.Show("Please select a role.");
                    return;
                }

                var user = await _authService.FindUserByUsernameAsync(selectedUserViewModel.UserName);
                var result = await _authService.ChangeUserRoleAsync(user, selectedRole);

                if (result)
                {
                    MessageBox.Show("User role updated successfully.");
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Failed to update user role.");
                }
            }
            else
            {
                MessageBox.Show("Please select a valid user.");
            }
        }

    }
}
