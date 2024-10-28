using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<InventoryUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(InventoryUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<InventoryUser> FindUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> ChangeUserRoleAsync(InventoryUser user, string newRole)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return false;
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            return addResult.Succeeded;
        }

        public string? GetCurrentUserName() => CurrentUser?.UserName;
    }
}
