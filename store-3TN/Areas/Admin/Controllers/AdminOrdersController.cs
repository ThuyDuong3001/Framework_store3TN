using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using store_3TN.Models;

namespace store_3TN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly store3TNContext _context;

        public AdminOrdersController(store3TNContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrders
        public async Task<IActionResult> Index(int page = 1, int CustomertID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;
            List<Order> lsOrder = new List<Order>();
            if (CustomertID != 0)
            {
                lsOrder = _context.Orders
                .AsNoTracking()
                .Where(x => x.CustomerId == CustomertID)
                .Include(x => x.Customer)
                .OrderBy(x => x.CustomerId).ToList();
            }
            else
            {
                lsOrder = _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .OrderBy(x => x.CustomerId).ToList();
            }
            PagedList<Order> cus = new PagedList<Order>(lsOrder.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentCus = CustomertID;
            ViewData["TenKhachHang"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            return View(cus);
        }

        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            var oderDetails = _context.OrderDetails.Where(x => x.OrderId == id).ToList();
            ViewBag.orderDetails = oderDetails;

            var products = _context.Products;
            ViewBag.products = products;

            var customers = _context.Customers;
            ViewBag.customers = customers;
            return View(order);
        }

        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Chờ giao hàng", Value = "Chờ giao hàng" });
            lsTrangThai.Add(new SelectListItem() { Text = "Đang giao hàng", Value = "Đang giao hàng" });
            lsTrangThai.Add(new SelectListItem() { Text = "Đã giao hàng", Value = "Đã giao hàng" });
            ViewData["lsTrangThai"] = lsTrangThai;
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatus,Deleted,Paid,TotalMoney,PaymentId,Note,Address,Discount,Province,District,Ward")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Chờ giao hàng", Value = "Chờ giao hàng" });
            lsTrangThai.Add(new SelectListItem() { Text = "Đang giao hàng", Value = "Đang giao hàng" });
            lsTrangThai.Add(new SelectListItem() { Text = "Đã giao hàng", Value = "Đã giao hàng" });
            ViewData["lsTrangThai"] = lsTrangThai;
            var customers = _context.Customers;
            ViewBag.customers = customers;

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatus,Deleted,Paid,TotalMoney,PaymentId,Note,Address,Discount,Province,District,Ward")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var customers = _context.Customers;
            ViewBag.customers = customers;
            return View(order);
        }

        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
