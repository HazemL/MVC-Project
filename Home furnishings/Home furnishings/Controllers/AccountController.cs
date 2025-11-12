
using Home_furnishings.Models;
using Home_furnishings.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Home_furnishings.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email
                };

                // Create user with hashed password
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                //cookeis
                // Sign in automatically after registration 
                await signInManager.SignInAsync(user, isPersistent: false);

                TempData["Success"] = "Registration successful!";
                return RedirectToAction("Index", "Account");
            }

            return View(model);
        }












        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser_ViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Login successful!";
                    return RedirectToAction("Index", "Home");  
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account is locked. Try again later.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View("Login", model);
                }
            }

            ModelState.AddModelError("", "Invalid login attempt. Please check your email or password.");
            return View(model);
        }


        //logout
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");

        }




        }
}
