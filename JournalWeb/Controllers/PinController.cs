using Microsoft.AspNetCore.Mvc;
using JournalWeb.Data;
using JournalWeb.Helpers;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace JournalWeb.Controllers
{
    public class PinController : Controller
    {
        private readonly AppDbContext _context;

        public PinController(AppDbContext context)
        {
            _context = context;
        }

        // ===== SET PIN =====
        public IActionResult Set()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Set(string pin)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length < 4 || pin.Length > 6 || !pin.All(char.IsDigit))
            {
                ViewBag.Error = "PIN phải từ 4–6 số";
                return View();
            }

            var userId = HttpContext.Session.GetInt32("NguoiDungId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = _context.NguoiDung.Find(userId.Value);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            user.PinHash = SecurityHelper.HashPin(pin);
            _context.SaveChanges();

            return RedirectToAction("Index", "NhatKy");
        }

        // ===== VERIFY PIN =====
        public IActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify(string pin)
        {
            var userId = HttpContext.Session.GetInt32("NguoiDungId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = _context.NguoiDung.Find(userId.Value);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            if (SecurityHelper.VerifyPin(pin, user.PinHash))
            {
                HttpContext.Session.SetString("PinVerified", "true");
                return RedirectToAction("Index", "NhatKy");
            }

            ViewBag.Error = "Sai PIN";
            return View();
        }
    }
}