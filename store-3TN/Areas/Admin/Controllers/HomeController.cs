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

namespace store_3TN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly store3TNContext _context;
        public HomeController(store3TNContext context) { _context = context; }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            // get account by id
            var account = _context.Accounts.Where(a => a.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            // if account is admin
            if (account.RoleId == 1)
            {
                var countProduct = _context.Products.ToList().Count();
                ViewBag.countProduct = countProduct;

                var salesProduct = _context.Orders.Sum(x => x.TotalMoney);
                ViewBag.salesProduct = salesProduct;

                var countOrder = _context.Orders.ToList().Count();
                ViewBag.countOrder = countOrder;

                var countCustomer = _context.Orders.ToList().Count();
                ViewBag.countCustomer = countCustomer;

                // select 4 product have bestseller=true
                var bestSeller = _context.Products.Where(p => p.BestSellers == true).Take(4).ToList();
                ViewBag.bestSeller = bestSeller;


                return View();
            }
            else
            {
                // return to error page in /Views/Shared/Error.cshtml
                return RedirectToAction("Error", "Home", new { area = "" });
            }

        }
    }
}
