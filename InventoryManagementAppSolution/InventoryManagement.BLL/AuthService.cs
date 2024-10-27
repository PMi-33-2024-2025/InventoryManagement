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

		public async Task<IdentityResult> RegisterUserAsync(string username, string password)
		{
			var user = new InventoryUser { UserName = username };
			return await _userManager.CreateAsync(user, password);
		}

		public async Task<SignInResult> LoginUserAsync(string username, string password)
		{
			return await _signInManager.PasswordSignInAsync(username, password, false, false);
		}

		public async Task LogoutUserAsync()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
