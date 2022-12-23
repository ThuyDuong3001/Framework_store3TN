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
    public class CommentController : Controller
    {
        private readonly store3TNContext _context;
        public CommentController(store3TNContext context)
        {
            _context = context;
        }
        public IActionResult AddComment(String comment, int productId, int? ReplyTo)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var customer = _context.Customers.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
                var newComment = new Comment()
                {
                    CustomerId = customer.CustomerId,
                    ProductId = productId,
                    Comment1 = comment,
                    CreatedDate = DateTime.Now,
                    ReplyTo = ReplyTo
                };
                _context.Comments.Add(newComment);
                _context.SaveChanges();
                // reload current page
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }
    }
}