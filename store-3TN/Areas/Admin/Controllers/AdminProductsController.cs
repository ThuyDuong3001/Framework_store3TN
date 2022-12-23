using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using PagedList.Core.Mvc;
using store_3TN.Models;

namespace store_3TN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notifyService { get; }

        public AdminProductsController(store3TNContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index(int page=1, int CatID =0)
        {        
            var pageNumber = page;
            var pageSize = 10;
            List<Product> lsProducts = new List<Product>();
            if (CatID != 0)
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.CatId == CatID)
                .Include(x => x.Cat)
                .OrderBy(x => x.ProductId).ToList();
            }
            else
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Include(x => x.Cat)
                .OrderBy(x => x.ProductId).ToList();
            }

            PagedList<Product> product = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentCatID=CatID;
            ViewData["LoaiSanPham"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View(product);
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["LoaiSanPham"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,Ram,Color,Storage,Details,CatId,Series,PriceOld,Price,Discount,Thumb,DateCreated,DateModified,BestSellers,HomeFlag,Active,UnitsInStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.DateCreated=DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                _notifyService.Success("Thêm mới thành công!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiSanPham"] = new SelectList(_context.Categories, "CatId", "CatName",product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["LoaiSanPham"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }
        //Filter categories
        public IActionResult Filtter(int CatID )
        {
            var url = $"/Admin/AdminProducts?CatId={CatID}";
            if (CatID == 0 )
            {
                url = $"/Admin/AdminProducts";
            }
           
            return Json(new { status = "success", redirectUrl = url });
        }
        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,Ram,Color,Storage,Details,CatId,Series,PriceOld,Price,Discount,Thumb,DateCreated,DateModified,BestSellers,HomeFlag,Active,UnitsInStock")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.DateModified = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Cập nhật thành công!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        _notifyService.Error("Có lỗi xảy ra!");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
