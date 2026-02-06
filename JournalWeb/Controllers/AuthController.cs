using Microsoft.AspNetCore.Mvc;
using JournalWeb.Data;
using JournalWeb.Models;
using JournalWeb.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace JournalWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ================= REGISTER =================
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
            string email,
            string hoTen,
            string matKhau,
            string confirmMatKhau,
            string pin,
            string confirmPin)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            if (matKhau != confirmMatKhau)
            {
                ViewBag.Loi = "Mật khẩu xác nhận không khớp";
                return View();
            }

            // PIN: đúng 4 số
            if (string.IsNullOrEmpty(pin) || pin.Length != 4 || !pin.All(char.IsDigit))
            {
                ViewBag.Loi = "Mã PIN phải gồm đúng 4 chữ số";
                return View();
            }

            if (pin != confirmPin)
            {
                ViewBag.Loi = "Xác nhận PIN không khớp";
                return View();
            }

            if (_context.NguoiDung.Any(x => x.Email == email))
            {
                ViewBag.Loi = "Email đã tồn tại";
                return View();
            }

            var user = new NguoiDung
            {
                Email = email,
                HoTen = hoTen,
                MatKhauHash = SecurityHelper.HashPassword(matKhau),
                PinHash = SecurityHelper.HashPin(pin),
                IsActive = true,
                NgayTao = DateTime.Now
            };

            _context.NguoiDung.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ================= LOGIN =================
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string matKhau)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var hash = SecurityHelper.HashPassword(matKhau);
            var user = _context.NguoiDung
                .FirstOrDefault(x => x.Email == email && x.MatKhauHash == hash);

            if (user == null)
            {
                ViewBag.Loi = "Email hoặc mật khẩu không đúng";
                return View();
            }

            if (!user.IsActive)
            {
                ViewBag.Loi = "Tài khoản đã bị khóa";
                return View();
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("NguoiDungId", user.NguoiDungId);
            HttpContext.Session.SetString("HoTen", user.HoTen ?? "");
            HttpContext.Session.SetString("PinVerified", "false");


            return RedirectToAction("VerifyPinLogin");
        }

        // ================= PIN SAU LOGIN =================
        public IActionResult VerifyPinLogin()
        {
            if (!HttpContext.Session.GetInt32("NguoiDungId").HasValue)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult VerifyPinLogin(string pin)
        {
            var userId = HttpContext.Session.GetInt32("NguoiDungId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = _context.NguoiDung.Find(userId.Value);

            if (!SecurityHelper.VerifyPin(pin, user.PinHash))
            {
                ViewBag.Loi = "Mã PIN không đúng";
                return View();
            }

            HttpContext.Session.SetString("PinVerified", "true");
            return RedirectToAction("Index", "NhatKy");
        }


        // ================= FORGOT PASSWORD =================
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.NguoiDung.FirstOrDefault(x => x.Email == email);
            if (user == null || string.IsNullOrEmpty(user.PinHash))
            {
                ViewBag.Loi = "Không thể xác thực tài khoản";
                return View();
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("ResetUserId", user.NguoiDungId);

            return RedirectToAction("VerifyPinReset");
        }

        // ================= PIN RESET PASSWORD =================
        public IActionResult VerifyPinReset()
        {
            if (!HttpContext.Session.GetInt32("ResetUserId").HasValue)
                return RedirectToAction("ForgotPassword");

            return View();
        }

        [HttpPost]
        public IActionResult VerifyPinReset(string pin)
        {
            var userId = HttpContext.Session.GetInt32("ResetUserId");
            if (!userId.HasValue)
                return RedirectToAction("ForgotPassword");

            var user = _context.NguoiDung.Find(userId.Value);

            if (!SecurityHelper.VerifyPin(pin, user.PinHash))
            {
                ViewBag.Loi = "Mã PIN không đúng";
                return View();
            }

            HttpContext.Session.SetString("PinVerifiedReset", "true");
            return RedirectToAction("ResetPassword");
        }

        // ================= RESET PASSWORD =================
        public IActionResult ResetPassword()
        {
            if (HttpContext.Session.GetString("PinVerifiedReset") != "true")
                return RedirectToAction("ForgotPassword");

            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string password, string confirm)
        {
            if (password != confirm)
            {
                ViewBag.Loi = "Mật khẩu không khớp";
                return View();
            }

            var userId = HttpContext.Session.GetInt32("ResetUserId");
            var user = _context.NguoiDung.Find(userId.Value);

            user.MatKhauHash = SecurityHelper.HashPassword(password);
            _context.SaveChanges();

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
