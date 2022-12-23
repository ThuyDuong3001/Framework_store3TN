using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_3TN.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;


namespace store_3TN.Controllers
{
    public class HomeController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notyfService { get; }
        public HomeController(store3TNContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // private readonly ILogger<HomeController> _logger;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }
        public IActionResult Index()
        {
            // select random 8 bestseller product
            var bestSeller = _context.Products.Where(p => p.BestSellers == true).Take(8).ToList();
            var homeFlag = _context.Products.Where(p => p.HomeFlag == true).ToList();
            ViewBag.bestSeller = bestSeller;
            ViewBag.homeFlag = homeFlag;
            ViewBag.FreeShip = _context.Vouchers.Where(s => s.VoucherType == 1).Take(3).ToList();
            ViewBag.Discount = _context.Vouchers.Where(s => s.VoucherType == 2).Take(3).ToList();
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
        public static string GetMD5(string str, bool isLowercase = true)
        {
            using (var md5 = MD5.Create())
            {
                var byteHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var hash = BitConverter.ToString(byteHash).Replace("-", "");
                return (isLowercase) ? hash.ToLower() : hash;
            }
        }
    }
}
