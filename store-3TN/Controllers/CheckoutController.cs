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
    public class CheckoutController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(store3TNContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [AllowAnonymous]
        [Route("checkout", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // find customer has account id
                var customer = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
                ViewBag.Cart = HttpContext.Session.Get<List<Cart>>("Cart");
                ViewBag.Vouchers = HttpContext.Session.Get<List<Voucher>>("Vouchers");
                return View(customer);
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("checkout", Name = "Checkout")]
        public async Task<IActionResult> Index(Customer customer)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            // if (taikhoanID == null)
            // {
            //     return RedirectToAction("Index", "Home");
            // }
            var customerID = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var cart = HttpContext.Session.Get<List<Cart>>("Cart");
                var vouchers = HttpContext.Session.Get<List<Voucher>>("Vouchers");
                var vc1 = 0;
                var vc2 = 0;
                if (vouchers != null)
                {
                    foreach (var item in vouchers)
                    {
                        if (item.VoucherType == 1 && item.Value > vc1)
                        {
                            vc1 = item.Value;
                        }
                        if (item.VoucherType == 2 && item.Value > vc2 && cart.Sum(c => c.Amount * c.Product.Price) > item.TotalMoneyRequire)
                        {
                            vc2 = item.Value;
                        }
                    }
                }
                var order = new Order()
                {
                    CustomerId = customerID.CustomerId,
                    OrderDate = DateTime.Now,
                    Province = customer.Province,
                    District = customer.District,
                    Ward = customer.Ward,
                    Address = customer.Address,
                    TotalMoney = cart.Sum(c => c.Amount * c.Product.Price) + 100000,
                    Discount = vc1 + vc2,
                    TransactStatus = "Chờ giao hàng",
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = item.Product.ProductId,
                        Amount = item.Amount,
                        Price = item.Product.Price,
                        TotalMoney = item.Amount * item.Product.Price

                    };
                    var product = _context.Products.Where(p => p.ProductId == item.Product.ProductId).FirstOrDefault();
                    product.UnitsInStock -= item.Amount;
                    _context.Products.Update(product);
                    _context.OrderDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");
                _notyfService.Success("Đặt hàng thành công");
                return RedirectToAction("Index", "Home");
            }
            return View(customer);
        }
    }
}
