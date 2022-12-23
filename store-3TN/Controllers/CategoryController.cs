using Microsoft.AspNetCore.Mvc;
using store_3TN.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PagedList.Core;
using System.Collections.Generic;
using System;
using store_3TN.Data;

namespace store_3TN.Controllers
{
    [Route("category")]
    public class CategoryController : Controller
    {
        private readonly store3TNContext _context;
        public CategoryController(store3TNContext context)
        {
            _context = context;
        }
        [Route("type/{id}"), ActionName("Index")]
        public IActionResult Index(int id, int? page, string color, string searchTxt)
        {
            var pageNumber = page == null || page < 0 ? 1 : page.Value;
            var pageSize = 9;
            var listProduct = _context.Products.AsNoTracking().Where(x => x.CatId == id).OrderByDescending(x => x.ProductId);
            if (!string.IsNullOrEmpty(searchTxt))
            {
                // find product in _context.Products contains searchTxt
                listProduct = _context.Products.AsNoTracking().Where(x => x.ProductName.Contains(searchTxt)).OrderByDescending(x => x.ProductId);
            }
            else
            {
                if (id == 0)
                {
                    listProduct = _context.Products.AsNoTracking().OrderByDescending(x => x.CatId);
                }
                if (color != null)
                {
                    listProduct = _context.Products.AsNoTracking().Where(x => x.Color == color).OrderByDescending(x => x.ProductId);
                }
            }
            int TotalPages = (int)Math.Ceiling((decimal)listProduct.Count() / (decimal)pageSize);
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
            PagedList<Product> category = new PagedList<Product>(listProduct, pageNumber, pageSize);
            ViewBag.Pager = category;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = TotalPages;
            ViewBag.StartPage = StartPage;
            ViewBag.EndPage = EndPage;
            // find list unique color in _context.Products
            var listColor = _context.Products.Select(x => x.Color).Distinct().ToList();
            ViewBag.listColor = listColor;
            // each item in Rating contains ProductId and Rate, calculate average rate for each product
            var listRating = _context.Ratings.ToList();
            ViewBag.listRating = listRating;
            return View(category);
        }
        [Route("{id}")]
        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index");
            }
            ViewBag.listComment = DBComments.getCommentByProductId(id);
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.RelatedProducts = _context.Products.Where(x => x.Series == product.Series).ToList();
            // get list rating of product
            var listRating = _context.Ratings.Where(x => x.ProductId == id).ToList();
            ViewBag.listRating = listRating;
            return View(product);
        }
    }
}
