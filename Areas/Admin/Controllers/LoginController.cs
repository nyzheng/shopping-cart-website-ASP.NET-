using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping.Areas.Admin.Models.ViewMembers;
using Shopping.Data;
using Shopping.Models;

namespace Shopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class LoginController : Controller
    {
		private DBContext _dbContext;
		private IPasswordHasher<Member> _passwordHasher;
	    public LoginController(DBContext dbContext,IPasswordHasher<Member> passwordHasher)
	    {
			_dbContext=dbContext;
			_passwordHasher=passwordHasher;
	    }

	    public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index( ViewLogin viewLogin)
        {
	        if (ModelState.IsValid)
	        {
		        Member member = _dbContext.Member.FirstOrDefault(x => x.Account == viewLogin.Account);
		        if (member != null)
		        {
			        var result = _passwordHasher.VerifyHashedPassword(member, member.Password, viewLogin.Password);
			        if (result == PasswordVerificationResult.Success)
			        {
				        var claims = new List<Claim>
				        {
					        new Claim(ClaimTypes.Name, member.Name),
					        new Claim(ClaimTypes.Sid, member.Id.ToString()),
					        new Claim(ClaimTypes.Email, member.Email)
				        };
				        var claimsIdentity = new ClaimsIdentity(claims, "ShoppingCookieAuth");
				        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
				        await HttpContext.SignInAsync("ShoppingCookieAuth", claimsPrincipal);
						return RedirectToAction("Index","Home");
			        }
		        }

	        }

	        ViewBag.message = "登入失敗!";
	        return View(viewLogin);
        }

        public async Task<IActionResult>  SignOutAccount()
        {
			await HttpContext.SignOutAsync("ShoppingCookieAuth");
	        return RedirectToAction("Index", "Login");
        }
	}
}
