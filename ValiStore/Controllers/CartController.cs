using Microsoft.AspNetCore.Mvc;
using ValiStore.Extention;
using ValiStore.Models;
using ValiStore.Models.ViewModels;
using ValiStore.Repository;

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
        /*[HttpPost]
        public IActionResult CheckOut(CheckOutViewModel Request)
        {
            return View();
        }*/
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
