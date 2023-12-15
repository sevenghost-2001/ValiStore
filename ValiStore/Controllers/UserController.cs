using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ValiStore.Models;
using System.Security.Claims;

namespace ValiStore.Controllers
{
    public class UserController : Controller
    {
        private readonly QLBanVaLiContext _context;

        public UserController(QLBanVaLiContext context)
        {
            _context = context;
        }

        // GET: User
        //public async Task<IActionResult> Index()
        //{
        //    var qLBanVaLiContext = _context.TKhachHangs.Include(t => t.UsernameNavigation);
        //    return View(await qLBanVaLiContext.ToListAsync());
        //}
        public  IActionResult Index()
        {
            var usernameClaim = User.FindFirst(ClaimTypes.Name);
            var username = usernameClaim != null ? usernameClaim.Value : "Guest";
            var khachhang = _context.TKhachHangs.Where(kh => kh.Username == username).ToList().FirstOrDefault();
            var hoadon = _context.THoaDonBans.Where(hd => hd.MaKhachHang == khachhang.MaKhanhHang).ToList();
            List<KhachHang_ChiTietHDB> viewModels = new List<KhachHang_ChiTietHDB>();
            if (hoadon.Count() >= 1)
            {
                foreach (var hd in hoadon)
                {
                    var cthd = _context.TChiTietHdbs.Where(cthd => cthd.MaHoaDon == hd.MaHoaDon).ToList();
                    var viewModel = new KhachHang_ChiTietHDB
                    {
                        hoaDonBan = hd,
                        khachHang = khachhang,
                        ChiTietHdbs = cthd
                    };
                    viewModels.Add(viewModel);
                };
            }
            else 
            {
                var viewModel = new KhachHang_ChiTietHDB
                {
                    khachHang = khachhang,
                };
                viewModels.Add(viewModel);
            } 
            return View(viewModels);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TKhachHangs == null)
            {
                return NotFound();
            }

            var tKhachHang = await _context.TKhachHangs
                .Include(t => t.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.MaKhanhHang == id);
            if (tKhachHang == null)
            {
                return NotFound();
            }

            return View(tKhachHang);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.TUsers, "Username", "Username");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,Username,TenKhachHang,NgaySinh,SoDienThoai,DiaChi,LoaiKhachHang,AnhDaiDien,GhiChu")] TKhachHang tKhachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tKhachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.TUsers, "Username", "Username", tKhachHang.Username);
            return View(tKhachHang);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TKhachHangs == null)
            {
                return NotFound();
            }

            var tKhachHang = await _context.TKhachHangs.FindAsync(id);
            if (tKhachHang == null)
            {
                return NotFound();
            }
            ViewData["Username"] = new SelectList(_context.TUsers, "Username", "Username", tKhachHang.Username);
            return View(tKhachHang);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhanhHang,Username,TenKhachHang,NgaySinh,SoDienThoai,DiaChi,LoaiKhachHang,AnhDaiDien,GhiChu")] TKhachHang tKhachHang)
        {
            if (id != tKhachHang.MaKhanhHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tKhachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TKhachHangExists(tKhachHang.MaKhanhHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.TUsers, "Username", "Username", tKhachHang.Username);
            return View(tKhachHang);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TKhachHangs == null)
            {
                return NotFound();
            }

            var tKhachHang = await _context.TKhachHangs
                .Include(t => t.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.MaKhanhHang == id);
            if (tKhachHang == null)
            {
                return NotFound();
            }

            return View(tKhachHang);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TKhachHangs == null)
            {
                return Problem("Entity set 'QLBanVaLiContext.TKhachHangs'  is null.");
            }
            var tKhachHang = await _context.TKhachHangs.FindAsync(id);
            if (tKhachHang != null)
            {
                _context.TKhachHangs.Remove(tKhachHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TKhachHangExists(string id)
        {
          return (_context.TKhachHangs?.Any(e => e.MaKhanhHang == id)).GetValueOrDefault();
        }
    }
}
