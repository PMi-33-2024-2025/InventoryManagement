using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.BLL
{
    public class AuthService
    {
        private readonly UserManager<InventoryUser> _userManager;
        public InventoryUser? CurrentUser { get; private set; }

        public AuthService(UserManager<InventoryUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterUserAsync(string username, string password)
        {
            var user = new InventoryUser { UserName = username };
            if (!(await _userManager.CreateAsync(user, password)).Succeeded)
            {
                throw new Exception("Failed to create user");
            }

            if (!(await _userManager.AddToRoleAsync(user, "User")).Succeeded)
            {
                throw new Exception("Failed to assign role");
            }
        }

        public async Task<bool> LoginUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (isPasswordValid)
            {
                CurrentUser = user;
                return true;
            }

            return false;
        }

        public bool IsUserLoggedIn() => CurrentUser != null;

        public async Task<bool> UserExists(string username)
        {
            return (await _userManager.FindByNameAsync(username)) is not null;
        }

        public void LogoutUser()
        {
            CurrentUser = null;
        }

        public async Task<string> GetCurrentUserRoleAsync()
        {
            if (CurrentUser is null)
            {
                return "Visitor";
            }

            var roles = await _userManager.GetRolesAsync(CurrentUser);
            return roles.FirstOrDefault() ?? "Visitor";
        }

        public string? GetCurrentUserName() => CurrentUser?.UserName;
    }
}
