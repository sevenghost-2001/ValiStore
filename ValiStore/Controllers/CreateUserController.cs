using Microsoft.AspNetCore.Mvc;
using ValiStore.Models;

namespace ValiStore.Controllers
{
    public class CreateUserController : Controller
    {
        private QLBanVaLiContext _context;

        public CreateUserController(QLBanVaLiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(TUser user)
        {
            if(_context.TUsers.Any(u => u.Username == user.Username)) 
            {
                ModelState.AddModelError("Username", "Tên người dùng đã tồn tại!");
                return View();
            }
            if(ModelState.IsValid)
            {
                _context.TUsers.Add(user);
                Guid guid = Guid.NewGuid();
                var khachhang = new TKhachHang();
                khachhang.MaKhachHang = guid.ToString("N").Substring(0,25);
                khachhang.Username = user.Username;
                _context.TKhachHangs.Add(khachhang);
                _context.SaveChanges();
                return RedirectToAction("Login", "Access");
            }
            return View();
        }
    }
}
