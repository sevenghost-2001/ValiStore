using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ValiStore.Models;

namespace ValiStore.Controllers
{
    public class AccessController : Controller
    {
        QLBanVaLiContext db = new QLBanVaLiContext();
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TUser user)
        {
            //if(db.TUsers.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault() != null)
            //{
            //    var claims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, user.Username),
            //    };
            //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    var principal = new ClaimsPrincipal(identity);

            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //    return RedirectToAction("Index", "Home");
            //}
            //ModelState.AddModelError("", "Invalid username or password");
            //return View();
            var user1 = db.TUsers.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
            if (user1 != null)
            {
                // Thêm vai trò cho người dùng
                var identity = new ClaimsIdentity("login");
                identity.AddClaim(new Claim(ClaimTypes.Name, user1.Username));
                identity.AddClaim(new Claim(ClaimTypes.Role, user1.LoaiUser.ToString())); // Chuyển đổi kiểu tinyint sang chuỗi

                var principal = new ClaimsPrincipal(identity);
                // Đăng nhập (sign in) người dùng
                HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login","Access");
        }
    }
}
