using InventoryManagement.DAL;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.BLL
{
    public class AuthService
    {
        private readonly UserManager<InventoryUser> _userManager;
        private readonly SignInManager<InventoryUser> _signInManager;

        public AuthService(UserManager<InventoryUser> userManager, SignInManager<InventoryUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(string username, string password)
        {
            var user = new InventoryUser { UserName = username };
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<SignInResult> LoginUserAsync(string username, string password)
        {
            return await _signInManager.PasswordSignInAsync(username, password, false, false);
        }
    }
}
