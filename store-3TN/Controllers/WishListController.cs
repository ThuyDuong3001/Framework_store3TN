using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_3TN.Models;
using System.Security.Cryptography;
using System.Text;

namespace store_3TN.Controllers
{
    public class WishListController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notyfService { get; }
        public WishListController(store3TNContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // find wishlist has account id
                var customerID = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
                var wishlist = _context.WishLists.Where(w => w.CustomerId == customerID.CustomerId).ToList();
                ViewBag.listProduct = _context.Products.ToList();
                return View(wishlist);
            }
            return View();
        }
        public IActionResult Remove(int ProductID)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            // find wishlist has account id
            var customerID = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            // remove wishlist has product id and customer id
            var wishlist = _context.WishLists.Where(w => w.CustomerId == customerID.CustomerId && w.ProductId == ProductID).FirstOrDefault();
            _context.WishLists.Remove(wishlist);
            _context.SaveChanges();
            _notyfService.Success("Đã xóa sản phẩm khỏi danh sách yêu thích");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult RemoveAll()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            // find wishlist has account id
            var customerID = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            // remove wishlist has customer id
            var wishlist = _context.WishLists.Where(w => w.CustomerId == customerID.CustomerId).ToList();
            _context.WishLists.RemoveRange(wishlist);
            _context.SaveChanges();
            _notyfService.Success("Đã xóa tất cả sản phẩm khỏi danh sách yêu thích");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Add(int ProductID)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            // find wishlist has account id
            var customerID = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            // add wishlist has product id and customer id
            var wishlist = new WishList();
            wishlist.CustomerId = customerID.CustomerId;
            wishlist.ProductId = ProductID;
            _context.WishLists.Add(wishlist);
            _context.SaveChanges();
            _notyfService.Success("Đã thêm sản phẩm vào danh sách yêu thích");
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}