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
    }
}
