using AspNetCoreHero.ToastNotification.Abstractions;
using store_3TN.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_3TN.Controllers
{
    public class VoucherController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notifyService { get; }
        public VoucherController(store3TNContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public List<Voucher> Vouchers
        {
            get
            {
                var vouchers = HttpContext.Session.Get<List<Voucher>>("Vouchers");
                if (vouchers == default(List<Voucher>))
                {
                    vouchers = new List<Voucher>();
                }
                return vouchers;
            }
        }
        public IActionResult AddVoucher(int VoucherID)
        {
            List<Voucher> vouchers = Vouchers;
            Voucher item = vouchers.Find(x => x.VoucherId == VoucherID);
            if (item == null)
            {
                // vouchers.Add(new Voucher
                // {
                //     VoucherId = VoucherID,
                // });
                // add voucher with voucherid in _context.Vouchers
                vouchers.Add(_context.Vouchers.Find(VoucherID));
            }
            else
            {
                _notifyService.Error("Bạn đã nhận Voucher này rồi");
                return Redirect(Request.Headers["Referer"].ToString());
            }
            HttpContext.Session.Set("Vouchers", vouchers);
            _notifyService.Success("Nhận thành công Voucher");
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}