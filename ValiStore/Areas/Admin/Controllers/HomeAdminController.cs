using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ValiStore.Extention;
using ValiStore.Models;
using X.PagedList;

namespace ValiStore.Areas.Admin.Controllers
{
   /* [Authorize(Policy = "AdminOnly")]*/
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QLBanVaLiContext db = new QLBanVaLiContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index(THoaDonBan hoaDonBan)
        {
            ViewBag.CountDH = new SelectList(db.THoaDonBans.ToList(), "MaHoaDon", "NgayHoaDon").Count();
            ViewBag.DT = db.THoaDonBans.Sum(x => x.TongTienHd);
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
        [Route("DanhsachDonHang")]
        public IActionResult DanhsachDonHang(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstDonHang = db.THoaDonBans.AsNoTracking().OrderBy(x => x.MaHoaDon);
            PagedList<THoaDonBan> lst = new PagedList<THoaDonBan>(lstDonHang, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhsachMauSac")]
        public IActionResult DanhsachMauSac(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstMau = db.TMauSacs.AsNoTracking().OrderBy(x => x.TenMauSac);
            PagedList<TMauSac> lst = new PagedList<TMauSac>(lstMau, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhsachDoiTuong")]
        public IActionResult DanhsachDoiTuong(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstDoiTuong = db.TLoaiDts.AsNoTracking().OrderBy(x => x.TenLoai);
            PagedList<TLoaiDt> lst = new PagedList<TLoaiDt>(lstDoiTuong, pageNumber, pageSize);
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
        public async Task<IActionResult> ThemSanPhamMoi(TDanhMucSp sanPham, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    sanPham.AnhDaiDien = await UploadFile.UploadFileImage(file, @"Images", file.FileName);
                }
                if (string.IsNullOrEmpty(sanPham.AnhDaiDien))
                {
                    sanPham.AnhDaiDien = "default-image.jpg";
                }
                db.TDanhMucSps.Add(sanPham);
                await db.SaveChangesAsync();
                return RedirectToAction("DanhMucSanPham");
            }
            return View();
        }
        [Route("ThemDoiTuongMoi")]
        [HttpGet]
        public IActionResult ThemDoiTuongMoi()
        {
            return View();
        }
        [Route("ThemDoiTuongMoi")]
        [HttpPost]
        public async Task<IActionResult> ThemDoiTuongMoi(TLoaiDt sanPham)
        {
            if (ModelState.IsValid)
            {

                db.TLoaiDts.Add(sanPham);
                await db.SaveChangesAsync();
                return RedirectToAction("DanhsachDoiTuong");
            }
            return View();
        }
        [Route("ThemMauSacMoi")]
        [HttpGet]
        public IActionResult ThemMauSacMoi()
        {
            return View();
        }
        [Route("ThemMauSacMoi")]
        [HttpPost]
        public async Task<IActionResult> ThemMauSacMoi(TMauSac sanPham)
        {
            if (ModelState.IsValid)
            {
                db.TMauSacs.Add(sanPham);
                await db.SaveChangesAsync();
                return RedirectToAction("DanhsachMauSac");
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
        public async Task<IActionResult> SuaSanPham(TDanhMucSp sanPham, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    sanPham.AnhDaiDien = await UploadFile.UploadFileImage(file, @"Images", file.FileName);
                }
                if (string.IsNullOrEmpty(sanPham.AnhDaiDien))
                {
                    sanPham.AnhDaiDien = "default-image.jpg";
                }
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }
            return View();
        }
        [Route("SuaDonHang")]
        [HttpGet]
        public IActionResult SuaDonHang(string maHoaDon)
        {
            ViewBag.MaKhachHang = new SelectList(db.TKhachHangs.ToList(), "MaKhanhHang", "TenKhachHang");
            ViewBag.MaNhanVien = new SelectList(db.TNhanViens.ToList(), "MaNhanVien", "TenNhanVien");
            var sanPham = db.THoaDonBans.Find(maHoaDon);
            return View(sanPham);
        }
        [Route("SuaDonHang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaDonHang(THoaDonBan sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DanhsachDonHang", "HomeAdmin");
            }
            return View();
        }
        [Route("SuaMauSac")]
        [HttpGet]
        public IActionResult SuaMauSac(string maMau)
        {
            var sanPham = db.TMauSacs.Find(maMau);
            return View(sanPham);
        }
        [Route("SuaMauSac")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaMauSac(TMauSac sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DanhsachMauSac", "HomeAdmin");
            }
            return View();
        }
        [Route("SuaDoiTuong")]
        [HttpGet]
        public IActionResult SuaDoiTuong(string maDT)
        {
            var sanPham = db.TLoaiDts.Find(maDT);
            return View(sanPham);
        }
        [Route("SuaDoiTuong")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaDoiTuong(TLoaiDt sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DanhsachDoiTuong", "HomeAdmin");
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
        [Route("XoaDonHang")]
        [HttpGet]
        public IActionResult XoaDonHang(string maHoaDon)
        {
            TempData["Message"] = "";
            db.Remove(db.THoaDonBans.Find(maHoaDon));
            db.SaveChanges();
            TempData["Message"] = "Hóa đơn đã được xóa";
            return RedirectToAction("DanhsachDonHang", "HomeAdmin");
        }
        [Route("XoaMauSac")]
        [HttpGet]
        public IActionResult XoaMauSac(string maMau)
        {
            TempData["Message"] = "";
            db.Remove(db.TMauSacs.Find(maMau));
            db.SaveChanges();
            TempData["Message"] = "Màu sắc đã được xóa";
            return RedirectToAction("DanhsachMauSac", "HomeAdmin");
        }
        [Route("XoaDoiTuong")]
        [HttpGet]
        public IActionResult XoaDoiTuong(string maDT)
        {
            TempData["Message"] = "";
            db.Remove(db.TLoaiDts.Find(maDT));
            db.SaveChanges();
            TempData["Message"] = "Màu sắc đã được xóa";
            return RedirectToAction("DanhsachDoiTuong", "HomeAdmin");
        }
        [Route("DanhSachLoaiSanPham")]
        public IActionResult DanhSachLoaiSanPham(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanpham = db.TLoaiSps.AsNoTracking().OrderBy(x => x.Loai);
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
        [Route("DanhsachKhachHang")]
        public IActionResult DanhsachKhachHang(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lsTKhachHang = db.TKhachHangs.AsNoTracking().OrderBy(x => x.TenKhachHang);
            PagedList<TKhachHang> lst = new PagedList<TKhachHang>(lsTKhachHang, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemKhachHangMoi")]
        [HttpGet]
        public IActionResult ThemKhachHangMoi()
        {
            ViewBag.Username = new SelectList(db.TUsers.ToList(), "Username", "Username");
            //ViewBag.LoaiUser = new SelectList(db.TUsers.ToList(), "Username", "LoaiUser");
            return View();
        }
        [Route("ThemKhachHangMoi")]
        [HttpPost]
        public IActionResult ThemKhachHangMoi(TKhachHang KhachHang)
        {
            if (ModelState.IsValid)
            {
                db.TKhachHangs.Add(KhachHang);
                db.SaveChanges();
                return RedirectToAction("DanhsachKhachHang");
            }
            return View();
        }
        [Route("SuaKhachHang")]
        [HttpGet]
        public IActionResult SuaKhachHang(string maKhanhHang)
        {
            ViewBag.Username = new SelectList(db.TUsers.ToList(), "Username", "Username");
            //ViewBag.LoaiUser = new SelectList(db.TUsers.ToList(), "Username", "LoaiUser");
            var KhachHang = db.TKhachHangs.Find(maKhanhHang);
            return View(KhachHang);
        }
        [Route("SuaKhachHang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaKhachHang(TKhachHang KhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(KhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhsachKhachHang", "HomeAdmin");
            }
            return View();
        }
        [Route("XoaKhachHang")]
        [HttpGet]
        public IActionResult XoaKhachHang(string maKhanhHang)
        {
            TempData["Message"] = "";
            //var chiTietSanPham = db.TChiTietSanPhams.Where(x => x.MaSp == maKhachHang).ToList();
            //if (chiTietSanPham.Count() > 0)
            //{
            //    TempData["Message"] = "Không xóa được sản phẩm này";
            //    return RedirectToAction("DanhsachKhachHang", "HomeAdmin");
            //}
            var anhDaiDien = db.TAnhSps.Where(x => x.MaSp == maKhanhHang);
            if (anhDaiDien.Any()) db.RemoveRange(anhDaiDien);
            db.Remove(db.TKhachHangs.Find(maKhanhHang));
            db.SaveChanges();
            TempData["Message"] = "Khách hàng đã được xóa";
            return RedirectToAction("DanhsachKhachHang", "HomeAdmin");
        }
        [Route("DanhsachNhanVien")]
        public IActionResult DanhsachNhanVien(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstNhanVien = db.TNhanViens.AsNoTracking().OrderBy(x => x.TenNhanVien);
            PagedList<TNhanVien> lst = new PagedList<TNhanVien>(lstNhanVien, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemNhanVienMoi")]
        [HttpGet]
        public IActionResult ThemNhanVienMoi()
        {
            ViewBag.Username = new SelectList(db.TUsers.ToList(), "Username", "Username");
            //ViewBag.LoaiUser = new SelectList(db.TUsers.ToList(), "Username", "LoaiUser");
            return View();
        }
        [Route("ThemNhanVienMoi")]
        [HttpPost]
        public IActionResult ThemNhanVienMoi(TNhanVien NhanVien)
        {
            if (ModelState.IsValid)
            {
                db.TNhanViens.Add(NhanVien);
                db.SaveChanges();
                return RedirectToAction("DanhsachNhanVien");
            }
            return View();
        }
        [Route("SuaNhanVien")]
        [HttpGet]
        public IActionResult SuaNhanVien(string maNhanVien)
        {
            ViewBag.Username = new SelectList(db.TUsers.ToList(), "Username", "Username");
            //ViewBag.LoaiUser = new SelectList(db.TUsers.ToList(), "Username", "LoaiUser");
            var NhanVien = db.TNhanViens.Find(maNhanVien);
            return View(NhanVien);
        }
        [Route("SuaNhanVien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaNhanVien(TNhanVien NhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(NhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhsachNhanVien", "HomeAdmin");
            }
            return View();
        }
        [Route("XoaNhanVien")]
        [HttpGet]
        public IActionResult XoaNhanVien(string maNhanVien)
        {
            TempData["Message"] = "";
            //var chiTietSanPham = db.TChiTietSanPhams.Where(x => x.MaSp == maKhachHang).ToList();
            //if (chiTietSanPham.Count() > 0)
            //{
            //    TempData["Message"] = "Không xóa được sản phẩm này";
            //    return RedirectToAction("DanhsachKhachHang", "HomeAdmin");
            //}
            //var anhDaiDien = db.TAnhSps.Where(x => x.MaSp == maNhanVien);
            //if (anhDaiDien.Any()) db.RemoveRange(anhDaiDien);
            db.Remove(db.TNhanViens.Find(maNhanVien));
            db.SaveChanges();
            TempData["Message"] = "Nhân viên đã được xóa";
            return RedirectToAction("DanhsachNhanVien", "HomeAdmin");
        }
    }
}
