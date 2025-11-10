using Home_furnishings.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Home_furnishings.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUser_ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // لو فيه أخطاء validation هيرجع نفس الصفحة ويعرضها
                return View(model);
            }

            // هنا تحط كود التسجيل الفعلي (مثلاً إنشاء مستخدم جديد)
            ViewBag.Success = "Registration successful!";
            return View();
        }










        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginUser_ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // لو فيه أخطاء Validation
                return View(model);
            }

            // ✅ هنا تقدر تضيف كود تسجيل الدخول الفعلي:
            // مثلاً:
            // var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            // لو فشل:
            // ModelState.AddModelError("", "Invalid login attempt.");

            if (model.Email == "admin@test.com" && model.Password == "Admin@123")
            {
                ViewBag.Success = "Login successful!";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return View(model);
        }




    }
}
