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
    public class RatingController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notifyService { get; }
        public RatingController(store3TNContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public IActionResult AddRating(String comment, int productId, int SoSao)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var customer = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
                // get all order of customer with transaction status = "Đã giao hàng"
                var orders = _context.Orders.Where(o => o.CustomerId == customer.CustomerId && o.TransactStatus == "Đã giao hàng").ToList();
                // get all order detail of these orders
                var orderDetails = new List<OrderDetail>();
                foreach (var order in orders)
                {
                    var orderDetail = _context.OrderDetails.Where(od => od.OrderId == order.OrderId).ToList();
                    orderDetails.AddRange(orderDetail);
                }
                // check if customer has bought this product
                var check = orderDetails.Where(od => od.ProductId == productId).FirstOrDefault();
                if (check == null)
                {
                    _notifyService.Error("Bạn chưa mua sản phẩm này");
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    var newRating = new Rating()
                    {
                        CustomerId = customer.CustomerId,
                        ProductId = productId,
                        Rate = SoSao,
                        CreateDate = DateTime.Now,
                        CmtContent = comment
                    };
                    // if customer has rated this product before, update the rating
                    var rating = _context.Ratings.Where(r => r.CustomerId == customer.CustomerId && r.ProductId == productId).FirstOrDefault();
                    if (rating != null)
                    {
                        rating.Rate = SoSao;
                        rating.CmtContent = comment;
                        rating.CreateDate = DateTime.Now;
                        _context.SaveChanges();
                        _notifyService.Success("Cập nhật đánh giá thành công");
                        return Redirect(Request.Headers["Referer"].ToString());
                    }
                    _context.Ratings.Add(newRating);
                    _context.SaveChanges();
                    // reload current page
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
        }
    }
}