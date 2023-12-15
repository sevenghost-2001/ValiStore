
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ValiStore.Models;
using ValiStore.Models.Authentication;
using X.PagedList;

namespace ValiStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        QLBanVaLiContext db = new QLBanVaLiContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        

        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstSanpham, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult Shop(string? search,string sortOrder, int? page)
        {
            ViewData["sortValue"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : sortOrder;
            ViewData["currentFilter"] = search;
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanpham = db.TDanhMucSps.AsNoTracking();
            if (!String.IsNullOrEmpty(search))
            {
                lstSanpham = lstSanpham.Where(sp => sp.TenSp.Contains(search));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    lstSanpham = lstSanpham.OrderByDescending(x => x.TenSp);
                    break;
                case "name_asc":
                    lstSanpham = lstSanpham.OrderBy(x => x.TenSp);
                    break;
                case "price_desc":
                    lstSanpham = lstSanpham.OrderByDescending(x => x.GiaLonNhat);
                    break;
                case "price_asc":
                    lstSanpham = lstSanpham.OrderBy(x => x.GiaLonNhat);
                    break;
                default:
                    lstSanpham = lstSanpham.OrderBy(x => x.GiaLonNhat);
                    break;
            }
            //var lstSanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new(lstSanpham, pageNumber, pageSize);
            return View(lst);
        }


        public IActionResult SanPhamTheoLoai(String maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanpham = db.TDanhMucSps.AsNoTracking().Where(x=>x.MaLoai==maloai).OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstSanpham, pageNumber, pageSize);
            ViewBag.maloai=maloai;
            return View(lst);
        }
        public IActionResult CHiTietSanPham(String maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp==maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            ViewBag.anhSanPham=anhSanPham;
            return View(sanPham);
        }
        
        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}