using Microsoft.AspNetCore.Mvc;
using store_3TN.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PagedList.Core;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace store_3TN.Controllers
{
    public class HistoryController : Controller
    {
        private readonly store3TNContext _context;
        public HistoryController(store3TNContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            var customerID = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            // get all order of customer
            var orders = _context.Orders.Where(o => o.CustomerId == customerID.CustomerId).ToList();
            ViewBag.Orders = orders;
            Dictionary<int, List<OrderDetail>> orderDetails = new Dictionary<int, List<OrderDetail>>();
            foreach (var order in orders)
            {
                var orderDetail = _context.OrderDetails.Where(od => od.OrderId == order.OrderId).ToList();
                orderDetails.Add(order.OrderId, orderDetail);
            }
            ViewBag.OrderDetails = orderDetails;
            Dictionary<int, Product> products = new Dictionary<int, Product>();
            var listProduct = _context.Products.ToList();
            foreach (var product in listProduct)
            {
                products.Add(product.ProductId, product);
            }
            ViewBag.Products = products;
            return View();
        }
    }
}