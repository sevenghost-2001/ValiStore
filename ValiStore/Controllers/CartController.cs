using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;
using ValiStore.Extention;
using ValiStore.Models;
using ValiStore.Models.ViewModels;
using ValiStore.Repository;
using System.Security.Claims;

namespace ValiStore.Controllers
{
    public class CartController : Controller
    {
        private readonly QLBanVaLiContext db;
        public CartController(QLBanVaLiContext db) 
        {
            this.db = db;
        }
        public List<CartItemModel> Cart
        {
            get
            {
                var cart = HttpContext.Session.Get<List<CartItemModel>>("Cart");
                if (cart == default(List<CartItemModel>))
                {
                    cart = new List<CartItemModel>();
                }
                return cart;
            }
        }
        [Route("cart.html")]
        public IActionResult Index()
        {
            List<CartItemModel> cartItems = Cart;
            var totalPrice = 0.0m;
            foreach (var item in Cart)
            {
                totalPrice +=(decimal)item.Total;
            }
            ViewBag.GrandTotal = totalPrice;
            return View(cartItems);
        }

        public IActionResult CheckOut()
        {
            List<CartItemModel> cartItems = Cart;
            var totalPrice = 0.0m;
            foreach (var item in Cart)
            {
                totalPrice += (decimal)item.Total;
            }
            ViewBag.GrandTotal = totalPrice;
            return View(cartItems);
        }
        [HttpPost]
        public IActionResult PlaceOrder()
        {
            if (ModelState.IsValid)
            {
                List<CartItemModel> cartItems = Cart;
                var totalPrice = 0.0m;
                foreach (var item in Cart)
                {
                    totalPrice += (decimal)item.Total;
                }
                Guid guid = Guid.NewGuid();
                DateTime currentTime = DateTime.Now;
                var username = User.FindFirst(ClaimTypes.Name).Value.ToString();
                var mkh = db.TKhachHangs.Where(u => u.Username.Equals(username)).ToList().FirstOrDefault().MaKhachHang;
                var mhd = guid.ToString("N").Substring(0, 25);
                // add hoa don
                var order = new THoaDonBan
                {
                    MaHoaDon = mhd,
                    NgayHoaDon = currentTime,
                    MaKhachHang = mkh.ToString(),
                    TongTienHd = totalPrice,
                };
                db.THoaDonBans.Add(order);
                db.SaveChanges();
                // add chi tiet hoa don
                foreach( var item in  Cart )
                {
                    THoaDonBan hoadon = db.THoaDonBans.Find(mhd);
                    TChiTietSanPham sanpham = db.TChiTietSanPhams.FirstOrDefault(sp => sp.MaSp == item.Product.MaSp);
                    if( hoadon != null && sanpham != null )
                    {
                        var orderDetail = new TChiTietHdb
                        {
                            MaHoaDon = hoadon.MaHoaDon,
                            MaChiTietSp = sanpham.MaChiTietSp,
                            SoLuongBan = item.Quantity,
                            DonGiaBan = item.Total
                        };
                        db.TChiTietHdbs.Add(orderDetail);
                        db.SaveChanges();
                    }
                };
            return RedirectToAction("thanksPage");
            }
            return RedirectToAction("CheckOut");
        }
        public IActionResult thanksPage()
        {
            return View();
        }
        public async Task<IActionResult> Add(string maSp)
        {
            TDanhMucSp product = db.TDanhMucSps.Find(maSp);
            List<CartItemModel> cart = Cart;
            CartItemModel cartItems = cart.Where(c => c.Product.MaSp == maSp).FirstOrDefault();

            if (cartItems == null)
            {
                cartItems = new CartItemModel
                {
                    Product = product,
                    Quantity = 1
                };
                cart.Add(cartItems);
            }
            else
            {
                cartItems.Quantity += 1;
            }

            HttpContext.Session.Set<List<CartItemModel>>("Cart", cart);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(string productId, int? quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItemModel>>("Cart");
            try
            {
                if (cart != null)
                {
                    CartItemModel cartItem = cart.SingleOrDefault(x => x.Product.MaSp == productId);
                    if (cartItem != null && quantity.HasValue)
                    {
                        cartItem.Quantity = quantity.Value;
                    }

                    HttpContext.Session.Set<List<CartItemModel>>("Cart", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("api/cart/remove")]
        public IActionResult RemoveCartItem(string productId)
        {
            try
            {
                List<CartItemModel> lscartItem = Cart;
                CartItemModel item = lscartItem.SingleOrDefault(x => x.Product.MaSp == productId);
                if (item != null)
                {
                    lscartItem.Remove(item);
                }

                HttpContext.Session.Set<List<CartItemModel>>("Cart", lscartItem);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }
    }
}
