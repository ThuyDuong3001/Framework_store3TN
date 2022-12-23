using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_3TN.Models;
using System.Security.Cryptography;
using System.Text;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace store_3TN.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly store3TNContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(store3TNContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        /*[HttpGet]
        [AllowAnonymous]*/
        // public IActionResult ValidatePhone(string Phone)
        // {
        //     try
        //     {
        //         var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
        //         if (khachhang != null)
        //             return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

        //         return Json(data: true);

        //     }
        //     catch
        //     {
        //         return Json(data: true);
        //     }
        // }
        // [HttpGet]
        // [AllowAnonymous]
        // public IActionResult ValidateEmail(string Email)
        // {
        //     try
        //     {
        //         var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
        //         if (khachhang != null)
        //             return Json(data: "Email : " + Email + " đã được sử dụng");
        //         return Json(data: true);
        //     }
        //     catch
        //     {
        //         return Json(data: true);
        //     }
        // }
        // [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        // public IActionResult Dashboard()
        // {
        //     var taikhoanID = HttpContext.Session.GetString("AccountId");
        //     if (taikhoanID != null)
        //     {
        //         var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
        //         if (khachhang != null)
        //         {
        //             var lsDonHang = _context.Orders
        //                 .Include(x => x.TransactStatus)
        //                 .AsNoTracking()
        //                 .Where(x => x.AccountId == khachhang.AccountId)
        //                 .OrderByDescending(x => x.OrderDate)
        //                 .ToList();
        //             ViewBag.DonHang = lsDonHang;
        //             return View(khachhang);
        //         }

        //     }
        //     return RedirectToAction("Login");
        // }
        public class CustomerAccount
        {
            public Customer Customer { get; set; }
            public Account Account { get; set; }
        }
        public class ChangePasswordVM
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("register", Name = "Register")]
        public IActionResult Register()
        {
            var model = new CustomerAccount
            {
                Customer = new Customer(),
                Account = new Account()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register", Name = "Register")]
        public async Task<IActionResult> Register(CustomerAccount taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // if UserName is exist
                    var account = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.UserName.ToLower() == taikhoan.Account.UserName.ToLower());
                    if (account != null)
                    {
                        _notyfService.Error("Tên đăng nhập đã tồn tại");
                        return View(taikhoan);
                    }
                    account = new Account
                    {
                        UserName = taikhoan.Account.UserName.Trim(),
                        PasswordHash = GetMD5(taikhoan.Account.PasswordHash.Trim()),
                        RoleId = 2,
                        Active = true,
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                    };
                    _context.Add(account);
                    await _context.SaveChangesAsync();

                    Customer customer = new Customer
                    {
                        AccountId = account.AccountId,
                        FullName = taikhoan.Customer.FullName.Trim(),
                        Phone = taikhoan.Customer.Phone == null ? null : taikhoan.Customer.Phone.Trim(),
                        Email = taikhoan.Customer.Email == null ? null : taikhoan.Customer.Email.Trim(),
                        Avatar = "https://ui-avatars.com/api/length=1&?rounded=true&?background=random&name=" + taikhoan.Customer.FullName.Trim(),
                    };
                    _context.Add(customer);
                    await _context.SaveChangesAsync();

                    try
                    {
                        // _context.Add(khachhang);
                        // await _context.SaveChangesAsync();
                        //Lưu Session MaKh
                        HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("AccountId");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,customer.FullName),
                            new Claim("AccountId", account.AccountId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        return RedirectToAction("Register", "Account");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        [AllowAnonymous]
        [Route("login", Name = "Login")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login", Name = "Login")]
        public async Task<IActionResult> Login(Account customer, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    // if (!isEmail) return View(customer);

                    var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.UserName == customer.UserName);

                    if (khachhang == null) return RedirectToAction("DangkyTaiKhoan");
                    string pass = GetMD5(customer.PasswordHash);
                    if (khachhang.PasswordHash != pass)
                    {
                        _notyfService.Error("Thông tin đăng nhập chưa chính xác");
                        return View(customer);
                    }
                    //kiem tra xem account co bi disable hay khong

                    if (khachhang.Active == false)
                    {
                        _notyfService.Error("Tài khoản của bạn đã bị khóa");
                        return View(customer);
                    }

                    //Luu Session MaKh
                    HttpContext.Session.SetString("AccountId", khachhang.AccountId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("AccountId");

                    //Identity
                    var claims = new List<Claim>
                    {
                        // new Claim(ClaimTypes.Name, khachhang.),
                        new Claim("AccountId", khachhang.AccountId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Đăng nhập thành công");
                    if (khachhang.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            catch
            {
                return RedirectToAction("Register", "Account");
            }
            return View(customer);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("AccountId");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("AccountId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Accounts.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    var pass = GetMD5(model.OldPassword);
                    {
                        string passnew = GetMD5(model.NewPassword);
                        {
                            if (pass == taikhoan.PasswordHash)
                            {
                                taikhoan.PasswordHash = passnew;
                                _context.Accounts.Update(taikhoan);
                                _context.SaveChanges();
                                _notyfService.Success("Đổi mật khẩu thành công");
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                _notyfService.Success("Mật khẩu cũ không đúng");
                                return View(model);
                            }
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }
        // [HttpPost]
        // public IActionResult ChangePassword(ChangePasswordViewModel model)
        // {
        //     try
        //     {
        //         var taikhoanID = HttpContext.Session.GetString("AccountId");
        //         if (taikhoanID == null)
        //         {
        //             return RedirectToAction("Login", "Accounts");
        //         }
        //         if (ModelState.IsValid)
        //         {
        //             var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
        //             if (taikhoan == null) return RedirectToAction("Login", "Accounts");
        //             var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
        //             {
        //                 string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
        //                 taikhoan.Password = passnew;
        //                 _context.Update(taikhoan);
        //                 _context.SaveChanges();
        //                 _notyfService.Success("Đổi mật khẩu thành công");
        //                 return RedirectToAction("Dashboard", "Accounts");
        //             }
        //         }
        //     }
        //     catch
        //     {
        //         _notyfService.Success("Thay đổi mật khẩu không thành công");
        //         return RedirectToAction("Dashboard", "Accounts");
        //     }
        //     _notyfService.Success("Thay đổi mật khẩu không thành công");
        //     return RedirectToAction("Dashboard", "Accounts");
        // }
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
