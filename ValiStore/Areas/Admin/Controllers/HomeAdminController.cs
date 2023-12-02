using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ValiStore.Models;
using X.PagedList;

namespace ValiStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QLBanVaLiContext db = new QLBanVaLiContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstSanpham, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        public IActionResult ThemSanPhamMoi(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View();
        }
        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSanPham)
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sanPham = db.TDanhMucSps.Find(maSanPham);
            return View(sanPham);
        }
        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham","HomeAdmin");
            }
            return View();
        }
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            TempData["Message"] = "";
            var chiTietSanPham = db.TChiTietSanPhams.Where(x => x.MaSp == maSanPham).ToList();
            if (chiTietSanPham.Count() > 0)
            {
                TempData["Message"] = "Không xóa được sản phẩm này";
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }
            var anhSanPhams = db.TAnhSps.Where(x => x.MaSp == maSanPham);
            if (anhSanPhams.Any()) db.RemoveRange(anhSanPhams);
            db.Remove(db.TDanhMucSps.Find(maSanPham));
            db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        }
        [Route("DanhSachLoaiSanPham")]
        public IActionResult DanhSachLoaiSanPham(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanpham = db.TLoaiSps.AsNoTracking().OrderBy(x=>x.Loai);
            PagedList<TLoaiSp> lst = new PagedList<TLoaiSp>(lstSanpham, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemLoaiMoi")]
        [HttpGet]
        public IActionResult ThemLoaiMoi()
        {
            return View();
        }
        [Route("ThemLoaiMoi")]
        [HttpPost]
        public IActionResult ThemLoaiMoi(TLoaiSp loaiSp)
        {
            if (ModelState.IsValid)
            {
                db.TLoaiSps.Add(loaiSp);
                db.SaveChanges();
                return RedirectToAction("DanhSachLoaiSanPham");
            }
            return View();
        }
        [Route("SuaLoaiSanPham")]
        [HttpGet]
        public IActionResult SuaLoaiSanPham(string maLoai)
        {
            var Loai = db.TDanhMucSps.Find(maLoai);
            return View(Loai);
        }
        [Route("SuaLoaiSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaLoaiSanPham(TLoaiSp loaiSp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiSp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachLoaiSanPham", "HomeAdmin");
            }
            return View();
        }
        [Route("XoaLoaiSanPham")]
        [HttpGet]
        public IActionResult XoaLoaiSanPham(string maLoai)
        {
            TempData["Message"] = "";
            var DanhMucSanPham = db.TDanhMucSps.Where(x => x.MaLoai == maLoai).ToList();
            if (DanhMucSanPham.Count() > 0)
            {
                TempData["Message"] = "Không xóa được loại sản phẩm này";
                return RedirectToAction("DanhSachLoaiSanPham", "HomeAdmin");
            }
            else
            {
                db.Remove(db.TLoaiSps.Find(maLoai));
                db.SaveChanges();
                TempData["Message"] = "Loại sản phẩm đã được xóa";
                return RedirectToAction("DanhSachLoaiSanPham", "HomeAdmin");
            }
        }
    }
}
