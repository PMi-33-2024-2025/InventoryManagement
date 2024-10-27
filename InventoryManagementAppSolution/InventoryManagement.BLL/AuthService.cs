using InventoryManagement.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.BLL
{
	public class AuthService
	{
		private readonly UserManager<InventoryUser> _userManager;
		private readonly SignInManager<InventoryUser> _signInManager;
		public static ServiceProvider ServiceProvider { get; set; }

		public AuthService(UserManager<InventoryUser> userManager, SignInManager<InventoryUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_signInManager.Context = new DefaultHttpContext { RequestServices = ServiceProvider };
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

		public async Task<SignInResult> LoginUserAsync(string username, string password)
		{
			return await _signInManager.PasswordSignInAsync(username, password, false, false);
		}

		public async Task<bool> UserExists(string username)
		{
			return (await _userManager.FindByNameAsync(username)) is not null;
		}

		public async Task LogoutUserAsync()
		{
			await _signInManager.SignOutAsync();
		}

		public async Task<InventoryUser?> GetCurrentLoggedInUserAsync()
		{
			return _signInManager.IsSignedIn(_signInManager.Context.User)
				? (await _userManager.GetUserAsync(_signInManager.Context.User))
				: null;
		}
	}
}
