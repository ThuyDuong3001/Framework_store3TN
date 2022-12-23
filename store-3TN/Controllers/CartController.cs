using AspNetCoreHero.ToastNotification.Abstractions;
using store_3TN.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_3TN.Controllers
{
    public class CartController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notifyService { get; }
        public CartController(store3TNContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public List<Cart> GioHang
        {
            get
            {
                var giohang = HttpContext.Session.Get<List<Cart>>("Cart");
                if (giohang == default(List<Cart>))
                {
                    giohang = new List<Cart>();
                }
                return giohang;
            }
        }
        public IActionResult AddToCart(int ProductID, int? Amount)
        {
            List<Cart> giohang = GioHang;
            Cart item = giohang.Find(x => x.Product.ProductId == ProductID);
            if (item == null)
            {
                giohang.Add(new Cart
                {
                    Product = _context.Products.Find(ProductID),
                    Amount = Amount ?? 1
                });
            }
            else
            {
                item.Amount += Amount ?? 1;
            }
            HttpContext.Session.Set("Cart", giohang);
            _notifyService.Success("Thêm vào giỏ hàng thành công");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult RemoveFromCart(int ProductID)
        {
            List<Cart> giohang = GioHang;
            Cart item = giohang.Find(x => x.Product.ProductId == ProductID);
            if (item != null)
            {
                giohang.Remove(item);
            }
            HttpContext.Session.Set("Cart", giohang);
            _notifyService.Success("Xóa khỏi giỏ hàng thành công");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult IncreaseAmount(int ProductID)
        {
            List<Cart> giohang = GioHang;
            Cart item = giohang.Find(x => x.Product.ProductId == ProductID);
            if (item != null)
            {
                item.Amount++;
            }
            HttpContext.Session.Set("Cart", giohang);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DecreaseAmount(int ProductID)
        {
            List<Cart> giohang = GioHang;
            Cart item = giohang.Find(x => x.Product.ProductId == ProductID);
            if (item != null)
            {
                if (item.Amount > 1)
                {
                    item.Amount--;
                }
            }
            HttpContext.Session.Set("Cart", giohang);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DeleteCart()
        {
            HttpContext.Session.Remove("Cart");
            // return home page
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
