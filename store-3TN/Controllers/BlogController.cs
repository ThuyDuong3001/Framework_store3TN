using Microsoft.AspNetCore.Mvc;
using store_3TN.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PagedList.Core;
using System.Collections.Generic;
using System;

namespace store_3TN.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly store3TNContext _context;
        public BlogController(store3TNContext context)
        {
            _context = context;
        }
        [Route("")]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page < 0 ? 1 : page.Value;
            var pageSize = 5;
            var listPost = _context.Posts.AsNoTracking().OrderByDescending(x => x.PostId);
            int TotalPages = (int)Math.Ceiling((decimal)listPost.Count() / (decimal)pageSize);
            int StartPage = pageNumber - 3;
            int EndPage = pageNumber + 3;
            if (StartPage <= 0)
            {
                EndPage -= (StartPage - 1);
                StartPage = 1;
            }
            if (EndPage > TotalPages)
            {
                EndPage = TotalPages;
                if (EndPage > 10)
                {
                    StartPage = EndPage - 9;
                }
            }
            PagedList<Post> blog = new PagedList<Post>(listPost, pageNumber, pageSize);
            ViewBag.Pager = blog;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = TotalPages;
            ViewBag.StartPage = StartPage;
            ViewBag.EndPage = EndPage;
            return View(blog);
        }
        [Route("{id}")]
        public IActionResult Details(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return RedirectToAction("index");
            }
            post.Views += 1;
            _context.Posts.Update(post);
            _context.SaveChanges();
            return View(post);
        }
    }
}
